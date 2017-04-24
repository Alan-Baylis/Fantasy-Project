using UnityEngine;
using System.Collections;
using System;

//This serves as a holder for the noise map of each chunk
public struct MapData {

    //Structs are supposed to be immutable, so readonly makes it so it can't be changed once it is assigned,
    //the heightmap holds height info for each coord in a chunk
    public readonly float[,] heightMap;

    //The constructor is the only place where we are allowed to assign the data
    public MapData(float[,] heightMap) {

        this.heightMap = heightMap;

    }
}

//Map thread info is a struct tha can hold a data type and a function call of a given data type T
public struct MapThreadInfo<T> {
    
    //callBack is a function that can be called later it takes in an object of type T as a parameter
    public readonly Action<T> callBack;
    //parameter is a data type of object type T that is inputed into callBak whenever it is to be called
    public readonly T parameter;

    //Assign the values in the constructor
    public MapThreadInfo(Action<T> callBack, T parameter) {
        this.callBack = callBack;
        this.parameter = parameter;
    }

}

//LOD = Level Of Detail
//LODMesh handles the requesting of meshData and generation of meshes from it for each mesh across threads
public class LODMesh {

    //The mesh of the meshData once it is received, it is written to later, so it can't be readonly
    public Mesh mesh;
    //Whether or not the mesh Data is being generated on another thread
    public bool hasRequestedMesh;
    //Whether or not the mesh Data has finished being generated on another thread
    public bool hasReceivedMesh;
    //The level of detail for this mesh
    private int lod;

    //The function to call once the mesh Data and mesh is fully generated
    private Action updateCallBack;

    //The constructor takes in the level of detail for the mesh and the function to call once it is complete
    public LODMesh(int lod, Action updateCallBack) {
        this.lod = lod;
        this.updateCallBack = updateCallBack;
    }

    //The function that starts the generation of the mesh on another thread, takes in the height map for the mesh and the map generator
    public void requestMesh(MapData mapData, MapGenerator mapGenerator) {

        //We have started generating a mesh in another thread
        hasRequestedMesh = true;
        //request meshData from another thread using the system in mapGenerator, the mesh will have a lod of lod and the function onMeshDataReceived() is
        //called once the data is fully generated
        mapGenerator.requestMeshData(mapData, lod, onMeshDataReceived);

    }

    //The function is called from another thread once the meshData has been computed for the inputed mapData
    void onMeshDataReceived(MeshData meshData) {

        //generate the actual mesh gameObject from the meshData generated in the other thread
        mesh = meshData.createMesh();
        //We have fully received all the data for the mesh
        hasReceivedMesh = true;
        //Call the function that we were given in the constructor
        updateCallBack();

    }

}

//LODInfo is used in the editor to change the level of detail to distance relationship for chunks
[Serializable]
public struct LODInfo {

    //The level of detail for the current distance
    public int lod;
    //The distance the chunk must be within to have this lod
    public float visibleDistanceThreshold;
    //Whether or not we use this lod to generate collider meshes
    public bool useForCollider;

}
