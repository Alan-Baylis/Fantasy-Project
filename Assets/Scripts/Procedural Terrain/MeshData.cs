using UnityEngine;
using System.Collections;

//This class stores data that represents one mesh in the game, it then can use that data to generate a mesh
public class MeshData {

    //The array of all vertices' positions in the mesh
    private Vector3[] vertices;
    //The index of each triangle in the mesh there are 6 times as many triangles as vertices
    private int[] triangles;

    //Uvs are texture coordinates for the mesh, this array stores the coordinate of the texture for each vertex
    private Vector2[] uvs;

    //Keep track of the total ammount of triangles in the mesh, and the index that the next triangle added will be assigned
    private int triangleIndex;
    //Same again, but if the triangle has a vertex that is an edge
    private int borderTrianlgeIndex;

    //The position array of all vertices that are not to be rendered but serve to have correct alignment of rendered edge vertices
    private Vector3[] borderVertices;
    //Indices of the border triangles, they are indexed using negative numbers
    private int[] borderTriangles;

    //The vector array of the normals used for lighting of each vertex, there is one for each triangle
    private Vector3[] bakedNormals;

    //Whether or not each triangle shares vertices across triangles or have their own vertices
    private bool useFlatShading;

    /// <summary>
    /// Constructor defines the size of the mesh and whether or not we flatShade
    /// </summary>
    /// <param name="verticesPerLine">The number of vertex points we have along the dimensions of the mesh</param>
    /// <param name="useFlatShading">Whether or not each triangle has unique or shared vertex vectors</param>
    public MeshData(int verticesPerLine, bool useFlatShading) {

        //The mesh is a square with the given number of vertices on each row = to it's dimensions
        vertices = new Vector3[verticesPerLine * verticesPerLine];
        //There is a uv for each vertex so the array has the same dimensions
        uvs = new Vector2[verticesPerLine * verticesPerLine];
        //Because triangle edges cross over vertices there are -1 on each side, but six times as many as there are vertices
        triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

        //The border vertices array holds an entry for all 4 sides of the mesh aswell as the 4 corner vertices at the edges
        borderVertices = new Vector3[verticesPerLine * 4 + 4];
        //There are 6 triangles for every vertice and we follow the 4 edges along the length of the mesh
        //So there are  6 * 4 = 24 along the length
        borderTriangles = new int[24 * verticesPerLine];
        //Assign...
        this.useFlatShading = useFlatShading;

    }

    /// <summary>
    /// This function saves the data to represent one vertex at the given index in the mesh, with position and uv supplied
    /// </summary>
    /// <param name="vertexPosition"></param>
    /// <param name="uv"></param>
    /// <param name="vertexIndex"></param>
    public void addVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex) {
        //If the index is negative, the new vertex is a border vertex
        if(vertexIndex < 0) {
            //with negative indexing you start att -1 not 0, so invert and +1 to get valid indexing
            //For border vertices we can ignore uvs
            borderVertices[-vertexIndex - 1] = vertexPosition;
        } else {
            //Otherwise the vertex is in the rendered mesh and we add the entries into the array
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }

    }

    /// <summary>
    /// We add points of a triangle in a clockwise fashion starting from the bottom right
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public void addTriangle(int a, int b, int c) {

        // b--c
        //  \ |
        //   \a

        //If any of the triangles positions added are a border vertex, we save this triangle to the border triangles array
        if(a < 0 || b < 0 || c < 0) {
            borderTriangles[borderTrianlgeIndex] = a;
            borderTriangles[borderTrianlgeIndex + 1] = b;
            borderTriangles[borderTrianlgeIndex + 2] = c;
            //Increment the index over the 3 added entries
            borderTrianlgeIndex += 3;
        } else {
            //The only difference is we add these to the actuall triangles array
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

    }

    /// <summary>
    /// Compute the layout of all vertices and points in the mesh based off of inputted info
    /// </summary>
    public void processMesh() {
        //If we are using flatshading we process the mesh giving each triangle 3 seperate vertices all pointing in the same direction
        if(useFlatShading) {
            flatShading();
        //Otherwise we make neighbouring triangles share vertices and their direction is the orthogonal vector to all of them 
        } else {
            bakeNormals();
        }
    }

    /// <summary>
    /// With flatshading we give each vertex it's own normal vector where the 3 vectors for the 3 triangle vertices all point in the same direction.
    /// With flatshading, we can ignore the border vertices since each triangle has seperate normals for lighting so we don't have to worry about hard edges
    /// </summary>
    private void flatShading() {

        //There is a verex for each triangle corner
        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        //The uvs are responsible for texturing and lighting each vertex so there is one for each vertex
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        //Iterate iver all the triangle edges in the mesh
        for(int i = 0; i < triangles.Length; i++) {

            //Get the vertex for the current triangle corner and add it to the array of vertices
            flatShadedVertices[i] = vertices[triangles[i]];
            //Get the uv for this vertex
            flatShadedUvs[i] = uvs[triangles[i]];
            //Set the vertex index for the triangle corner to be this index
            triangles[i] = i;
        }

        //Assign the vertices and uvs
        vertices = flatShadedVertices;
        uvs = flatShadedUvs;

    }

    /// <summary>
    /// Calculate the lighting normals for the mesh and assign them to the normals array
    /// </summary>
    private void bakeNormals() {
        bakedNormals = calculateNormals();
    }

    /// <summary>
    /// Each triangles face has a normal that is 90 deg to it that is used for lighting the face, the normal is calculated by finding the average direction from the 3 vertex
    ///vectors
    /// </summary>
    /// <returns></returns>
    public Vector3[] calculateNormals() {

        //Create a new array to hold the normals
        Vector3[] vertexNormals = new Vector3[vertices.Length];
        //There are 3 corners on a triangle for every face
        int triangleCount = triangles.Length / 3;
        //Iterate over the triangle faces
        for(int i = 0; i < triangleCount; i++) {
            //
            int normalTriangleIndex = i * 3;
            //Get all 3 vertices' indexes that represent the corners of the triangle
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            //Calculate the normal using these vertex vectors
            Vector3 triangleNormal = surfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

            //Add the normal to the array for these 3 vertex positions
            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;

        }

        //Same face to triangles ratio
        int borderTriangleCount = borderTriangles.Length / 3;
        for(int i = 0; i < borderTriangleCount; i++) {

            //Repeat the whole get the normal again but use border vertices
            int normalTriangleIndex = i * 3;
            int vertexIndexA = borderTriangles[normalTriangleIndex];
            int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
            int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = surfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
            //If any of these vertices are also to be rendered in the mesh, add their normals to the render mesh vertex array
            if(vertexIndexA >= 0) {
                vertexNormals[vertexIndexA] += triangleNormal;
            }
            if(vertexIndexB >= 0) {
                vertexNormals[vertexIndexB] += triangleNormal;
            }
            if(vertexIndexC >= 0) {
                vertexNormals[vertexIndexC] += triangleNormal;
            }
        }

        //Iterate over all of the newly created normals and make them have a magnitude of 1
        foreach(Vector3 vertexNormal in vertexNormals) {
            vertexNormal.Normalize();
        }

        return vertexNormals;

    }

    /// <summary>
    /// This function calculates the normal from the 3 vectors of the triangles vertices
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public Vector3 surfaceNormalFromIndices(int a, int b, int c) {

        //If the index is negative get a border vertex otherwise get a normal vertex for this index
        Vector3 pointA = (a < 0) ? borderVertices[-a - 1] : vertices[a];
        Vector3 pointB = (b < 0) ? borderVertices[-b - 1] : vertices[b];
        Vector3 pointC = (c < 0) ? borderVertices[-c - 1] : vertices[c];

        //The two relative vectors that point from A to B & A to C represent a side of the triangle and their magnitude is the side's length
        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;

        //The normal is the vector that is mutually orthogonal to the surface of the triangle face and has magnitude 1
        return Vector3.Cross(sideAB, sideAC).normalized;

    }

    /// <summary>
    /// Converts all of the triangles, vertices and uv data stored in meshData into an actual mesh object
    /// </summary>
    /// <returns>The Mesh utilising all of the inputed data</returns>
    public Mesh createMesh() {

        //Create a new empty mesh to display all the data
        Mesh mesh = new Mesh();

        //Assign the vertices, triangles and uvs arrays to the mesh
        mesh.vertices = this.vertices;
        mesh.triangles = this.triangles;
        mesh.uv = this.uvs;

        //Assign the normals based off of whether we use flatshading or not
        if(useFlatShading) {
            mesh.RecalculateNormals();
        } else {
            mesh.normals = bakedNormals;
        }

        //return the populated mesh
        return mesh;

    }

}