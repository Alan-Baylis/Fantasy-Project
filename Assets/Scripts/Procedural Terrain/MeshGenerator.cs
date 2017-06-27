using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

    /// <summary>
    /// This function generates meshData for a mesh using the inputed data
    /// </summary>
    /// <param name="borderedSizeMap">the map of all height values for the mesh</param>
    /// <param name="heighMultiplier">to scale up the heights of the mesh</param>
    /// <param name="_borderedSizeCurve">The Animation curve gives an alternate height value for the each heightMap value to fit the terrain better</param>
    /// <param name="levelOfDetail">levelOfDetail affects how many vertices are in the mesh</param>
    /// <param name="useFlatShading">useFlatShading allows each triangle to have it's own 3 vertices so the entire triangle's face is lit uniformly</param>
    /// <returns></returns>
    public static MeshData generateTerrainMesh(float[,] borderedSizeMap, float heighMultiplier, AnimationCurve _borderedSizeCurve, int levelOfDetail, bool useFlatShading) {

        //Create a copy of the AnimationCurve so that it can be read across multiple threads
        AnimationCurve borderedSizeCurve = new AnimationCurve(_borderedSizeCurve.keys);
        //The number of verices that are skipped over, if it is 1, we are at full detail
        //If the Lod = 0, the increment is 1, otherwise it's twice the supplied LOD
        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

        //The assumption is that the heightMap is square, so the length = width
        int borderedSize = borderedSizeMap.GetLength(0);
        //The size of the mesh is a vertex smaller on each end since there are non rendered vertices that serve to align the edge vertices properly,
        int meshSize = borderedSize - 2 * meshSimplificationIncrement;
        //Save a reference to the default size of the map if it's LOD = 0
        int meshSizeUnsimplified = borderedSize - 2;

        //These two variables represent the topLeft x & z coordinates of the mesh and are used to centre vertices in the mesh
        //divide by 2 to get the centre of the coordinate, -2 for x because otherwise it would be centred around the wrong axis
        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

        //The number of vertices that would be along the edge of the mesh, the higher the simplification increment, the less vertices
        int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;
        //Create a new empty meshData with the given number of vertices in a line and whether or not each triangle shares vertices or has it's own unique ones
        MeshData meshData = new MeshData(verticesPerLine, useFlatShading);

        //Create a new map for all vertices positions in the mesh of the given mesh size
        int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
        //Start the vertex index at 0
        int meshVertexIndex = 0;
        //The extra border of vertices all around the mesh are numberered from -1 -> -n, so start indexing at -1
        int borderVertexIndex = -1;

        //Iterate over all coordinates in the mesh, counting up by the simplification increment
        for(int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
            for(int x = 0; x < borderedSize; x += meshSimplificationIncrement) {
                //The vertex is on the border is it has a coordinate at 0, or is one of the last coordinates
                bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

                //If it's a border vertex, then we use the border vertices and decrement the count
                if(isBorderVertex) {
                    vertexIndicesMap[x, y] = borderVertexIndex;
                    borderVertexIndex--;
                    //Otherwise we use normal vertices and increment that count
                } else {
                    vertexIndicesMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }

            }
        }

        //Iterate over all the coordinates in the mesh again
        for(int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
            for(int x = 0; x < borderedSize; x += meshSimplificationIncrement) {

                //Get the vertex index as calulated earlier for this vertex
                int vertexIndex = vertexIndicesMap[x, y];

                //The percent represents how far throught along the x & z axes of the mesh we are, if we are the first entry its ~ -1 & the last is ~0
                Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float) meshSize, (y - meshSimplificationIncrement) / (float) meshSize);

                //The actual height of this vertex is the value from the HeightCurve for the heightMap at this coord scaled up by the multiplier
                float meshHeight = borderedSizeCurve.Evaluate(borderedSizeMap[x, y]) * heighMultiplier;

                //The position of the vertex is centred around the top left of the mesh, then shifted away from it by it's percent along the edge scaled up to the mesh size, for the x & z
                //the y coord is the vertex's height
                Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, meshHeight, topLeftZ - percent.y * meshSizeUnsimplified);

                //Add this vertex to the meshData at it's index, percent also serves to represent the texture coordinates of this vertex
                meshData.addVertex(vertexPosition, percent, vertexIndex);

                //If this coord is within the mesh and not one of the unrendered edge vertices, add a triangle for this coord
                if(x < borderedSize - 1 && y < borderedSize - 1) {

                    //Since we start at the top left of the mesh, the surrounding entries required to make 2 triangles are
                    //The x-z coord aswell as the neighbouring vertices that are one increment away in both directions:

                    //  a -> b     a\       a--b
                    //  | \  |  =  | \   &   \ |
                    //  v  \ v     c--d       \d
                    //  c    d

                    int a = vertexIndicesMap[x, y];
                    int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
                    int c = vertexIndicesMap[x, y + meshSimplificationIncrement];
                    int d = vertexIndicesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];

                    //Add the triangles as depicted in the diagram above
                    meshData.addTriangle(a, d, c);
                    meshData.addTriangle(d, a, b);
                }
                //Increment to the next vertex
                vertexIndex++;

            }
        }
        //Calculate the normals for the mesh (Normals = 90 deg vectors from face of vertex) used for lighting the mesh
        meshData.processMesh();

        //return the created meshData
        return meshData;

    }

}
