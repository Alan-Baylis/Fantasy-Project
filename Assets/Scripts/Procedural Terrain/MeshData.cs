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

    //Constructor defines the size of the mesh and whether or not we flatShade
    public MeshData(int verticesPerLine, bool useFlatShading) {

        //The mesh is a square with the given number of vertices on each row = to it's dimensions
        vertices = new Vector3[verticesPerLine * verticesPerLine];
        //There is a uv for each vertex so the array has the same dimensions
        uvs = new Vector2[verticesPerLine * verticesPerLine];
        //Because triangle edges cross over vertices there are -1 on each side, but six times as many as there are vertices
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