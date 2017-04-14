using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator {

    public static MeshData generateTerrainMesh(float[,] borderedSizeMap, float heighMultiplier, AnimationCurve _borderedSizeCurve, int levelOfDetail, bool useFlatShading) {

        AnimationCurve borderedSizeCurve = new AnimationCurve(_borderedSizeCurve.keys);
        int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

        int borderedSize = borderedSizeMap.GetLength(0);
        int meshSize = borderedSize - 2 * meshSimplificationIncrement;
        int meshSizeUnsimplified = borderedSize - 2;

        float topLeftX = (meshSizeUnsimplified - 1) / -2f;
        float topLeftZ = (meshSizeUnsimplified - 1) / 2f;

        int verticesPerLine = (meshSize - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, useFlatShading);

        int[,] vertexIndicesMap = new int[borderedSize, borderedSize];
        int meshVertexIndex = 0;
        int borderVertexIndex = -1;

        for(int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
            for(int x = 0; x < borderedSize; x += meshSimplificationIncrement) {

                bool isBorderVertex = y == 0 || y == borderedSize - 1 || x == 0 || x == borderedSize - 1;

                if(isBorderVertex) {
                    vertexIndicesMap[x, y] = borderVertexIndex;
                    borderVertexIndex--;
                } else {
                    vertexIndicesMap[x, y] = meshVertexIndex;
                    meshVertexIndex++;
                }

            }
        }

        for(int y = 0; y < borderedSize; y += meshSimplificationIncrement) {
            for(int x = 0; x < borderedSize; x += meshSimplificationIncrement) {

                int vertexIndex = vertexIndicesMap[x, y];

                Vector2 percent = new Vector2((x - meshSimplificationIncrement) / (float) meshSize, (y - meshSimplificationIncrement) / (float) meshSize);

                float meshHeight = borderedSizeCurve.Evaluate(borderedSizeMap[x, y]) * heighMultiplier;
                Vector3 vertexPosition = new Vector3(topLeftX + percent.x * meshSizeUnsimplified, meshHeight, topLeftZ - percent.y * meshSizeUnsimplified);

                meshData.addVertex(vertexPosition, percent, vertexIndex);

                if(x < borderedSize - 1 && y < borderedSize - 1) {

                    int a = vertexIndicesMap[x, y];
                    int b = vertexIndicesMap[x + meshSimplificationIncrement, y];
                    int c = vertexIndicesMap[x, y + meshSimplificationIncrement];
                    int d = vertexIndicesMap[x + meshSimplificationIncrement, y + meshSimplificationIncrement];

                    meshData.addTriangle(a, d, c);
                    meshData.addTriangle(d, a, b);
                }

                vertexIndex++;

            }
        }

        meshData.processMesh();

        return meshData;

    }

}

public class MeshData {

    private Vector3[] vertices;
    private int[] triangles;

    private Vector2[] uvs;

    private int triangleIndex;
    private int borderTrianlgeIndex;

    private Vector3[] borderVertices;
    private int[] borderTriangles;

    private Vector3[] bakedNormals;

    private bool useFlatShading;

    public MeshData(int verticesPerLine, bool useFlatShading) {

        vertices = new Vector3[verticesPerLine * verticesPerLine];
        uvs = new Vector2[verticesPerLine * verticesPerLine];
        triangles = new int[(verticesPerLine - 1) * (verticesPerLine - 1) * 6];

        borderVertices = new Vector3[verticesPerLine * 4 + 4];
        borderTriangles = new int[24 * verticesPerLine];

        this.useFlatShading = useFlatShading;

    }

    public void addVertex(Vector3 vertexPosition, Vector2 uv, int vertexIndex) {

        if(vertexIndex < 0) {
            borderVertices[-vertexIndex - 1] = vertexPosition;
        } else {
            vertices[vertexIndex] = vertexPosition;
            uvs[vertexIndex] = uv;
        }

    }

    public void addTriangle(int a, int b, int c) {

        if(a < 0 || b < 0 || c < 0) {
            borderTriangles[borderTrianlgeIndex] = a;
            borderTriangles[borderTrianlgeIndex + 1] = b;
            borderTriangles[borderTrianlgeIndex + 2] = c;
            borderTrianlgeIndex += 3;
        } else {
            triangles[triangleIndex] = a;
            triangles[triangleIndex + 1] = b;
            triangles[triangleIndex + 2] = c;
            triangleIndex += 3;
        }

    }

    public void processMesh() {
        if(useFlatShading) {
            flatShading();
        } else {
            bakeNormals();
        }
    }

    private void bakeNormals() {
        bakedNormals = calculateNormals();
    }

    private void flatShading() {

        Vector3[] flatShadedVertices = new Vector3[triangles.Length];
        Vector2[] flatShadedUvs = new Vector2[triangles.Length];

        for(int i = 0; i < triangles.Length; i++) {

            flatShadedVertices[i] = vertices[triangles[i]];
            flatShadedUvs[i] = uvs[triangles[i]];

            triangles[i] = i;
        }

        vertices = flatShadedVertices;
        uvs = flatShadedUvs;

    }

    public Vector3[] calculateNormals() {

        Vector3[] vertexNormals = new Vector3[vertices.Length];
        int triangleCount = triangles.Length / 3;
        for(int i = 0; i < triangleCount; i++) {

            int normalTriangleIndex = i * 3;
            int vertexIndexA = triangles[normalTriangleIndex];
            int vertexIndexB = triangles[normalTriangleIndex + 1];
            int vertexIndexC = triangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = surfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);

            vertexNormals[vertexIndexA] += triangleNormal;
            vertexNormals[vertexIndexB] += triangleNormal;
            vertexNormals[vertexIndexC] += triangleNormal;

        }

        int borderTriangleCount = borderTriangles.Length / 3;
        for(int i = 0; i < borderTriangleCount; i++) {

            int normalTriangleIndex = i * 3;
            int vertexIndexA = borderTriangles[normalTriangleIndex];
            int vertexIndexB = borderTriangles[normalTriangleIndex + 1];
            int vertexIndexC = borderTriangles[normalTriangleIndex + 2];

            Vector3 triangleNormal = surfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
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

        foreach(Vector3 vertexNormal in vertexNormals) {
            vertexNormal.Normalize();
        }

        return vertexNormals;

    }

    public Vector3 surfaceNormalFromIndices(int a, int b, int c) {

        Vector3 pointA = (a < 0) ? borderVertices[-a - 1] : vertices[a];
        Vector3 pointB = (b < 0) ? borderVertices[-b - 1] : vertices[b];
        Vector3 pointC = (c < 0) ? borderVertices[-c - 1] : vertices[c];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;

        return Vector3.Cross(sideAB, sideAC).normalized;

    }

    public Mesh createMesh() {

        Mesh mesh = new Mesh();

        mesh.vertices = this.vertices;
        mesh.triangles = this.triangles;
        mesh.uv = this.uvs;

        if(useFlatShading) {
            mesh.RecalculateNormals();
        } else {
            mesh.normals = bakedNormals;
        }

        return mesh;

    }

}
