using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    //The three available types of displaying the generated preview in the editor
    public enum DrawMode {
        //NOISEMAP is the black-and-white base rendering of the noiseMap generated
        NOISEMAP,
        //The falloff map is used to create small seperate islands on each mesh, this previews the falloff map
        FALLOFFMAP,
        //Mesh is the final view of the map in the game
        MESH
    };

    //The type of map to preview in the editor
    public DrawMode drawMode;

    //Whether or not to auto update the display changes in the editor
    public bool autoUpdate;

    //An object to hold all data relating to the terrain 
    public TerrainData terrainData;
    //An object that holds all data for terrain generation
    public NoiseData noiseData;
    //An object that holds all data for texturing the mesh
    public TextureData textureData;

    //The material that is to be applied to the mesh
    public Material terrainMaterial;

    //The size (length = width) of each mesh in the game, flatshading uses 3 times as many vertices, that is why it is a smaller mesh size
    public int mapChunkSize {
        get {
            if(terrainData.useFlatShading) {
                return 95;
            } else {
                //return 239;
                return 59;
            }
        }
    }

    //The inverse / 2 of the level of detail in the mesh
    //Actual values are {0, 1/2, 1/4, 1/6, 1/8, 1/10, 1/12}
    [Range(0, 6)]
    public int editorLOD;

    //The list of all generated mapdata that were generated on a seperate thread and added here to be created in game
    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    //Same again for mesh data
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    //The falloff map used to generate "continents" on each mesh
    private float[,] falloffMap;
    //Variables for the formula to generate a fall off map
    public float falloffCurveA;
    public float falloffCurveB;

    //This function is called whenever an above value is changed and it reflects those cahnges in the editor
    private void onValuesUpdated() {
        //If the game is playing, update the editor with the changes
        if(!Application.isPlaying) {
            drawMapInEditor();
        }

    }

    //Whenever the data in textureData changes, this function is called to update the material with the changes
    private void onTextureValuesUpdated() {
        textureData.applyToMaterial(terrainMaterial);
    }

    //This function is used to preview all changes made to the map generating functionality in the editor
    public void drawMapInEditor() {

        //Generate the mapData centered around the origin
        MapData mapData = generateMapData(Vector2.zero);

        //Get the mapDisplay script
        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        //If the NOISEMAP enum has been selected, we display the noisemap in the editor
        if(drawMode == DrawMode.NOISEMAP) {
            //Generate the texture for the heightmap
            Texture2D texture = TextureGenerator.textureFromHeightMap(mapData.heightMap);
            //Apply the generated texture to the plane game object in map display
            mapDisplay.drawTexture(texture);

            //If the MESH enum has been selected, we generate the mesh in the editor
        } else if(drawMode == DrawMode.MESH) {
            //Generate the meshData necessary for the mesh
            MeshData meshData = MeshGenerator.generateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorLOD, terrainData.useFlatShading);
            //Apply the mesh to the mesh game object in map display
            mapDisplay.drawMesh(meshData);

            //If the FALLOFFMAP enum has been selected, we draw a black-and-white texture similar to the noisemap
        } else if(drawMode == DrawMode.FALLOFFMAP) {

            falloffMap = FalloffGenerator.generateFalloffMap(mapChunkSize + 2, falloffCurveA, falloffCurveB);
            Texture2D texture = TextureGenerator.textureFromHeightMap(falloffMap);
            mapDisplay.drawTexture(texture);

        }

    }

    //We utitlise threading to generate the MapData, this function is called and starts the generation of the map data in a seperate thread
    //An Action is a function call that can be stored and called later
    public void requestMapData(Vector2 centre, Action<MapData> callBack) {

        //Create the thread and in it run the mapDataThread() function
        ThreadStart threadStart = delegate {
            mapDataThread(centre, callBack);
        };
        //run the thread
        new Thread(threadStart).Start();

    }

    //This function is called in a seperate thread so that the game doesn't jam up as we generate lots of mapData objects
    private void mapDataThread(Vector2 centre, Action<MapData> callBack) {

        //Generate the mapData, all functions called in a thread run in this thread
        MapData mapData = generateMapData(centre);
        //Save this data to the MapThreadInfo object, it simply stores the data so that it can cross threads, with the given function and
        //The mapData as the parameter for that function
        MapThreadInfo<MapData> mapThreadInfo = new MapThreadInfo<MapData>(callBack, mapData);
        //lock prevents multiple threads from accessing an object at the same time
        //Add the mapThreadInfo to the Queue (like a list) of mapThreadInfo, where it can be accessed from the main thread
        lock(mapDataThreadInfoQueue) {
            mapDataThreadInfoQueue.Enqueue(mapThreadInfo);
        }
    }

    //It's the exact same threading idea for meshData
    public void requestMeshData(MapData mapData, int levelOfDetail, Action<MeshData> callBack) {

        //Start the meshData generation in another thread and run the thread
        ThreadStart threadStart = delegate {
            meshDataThread(mapData, levelOfDetail, callBack);
        };
        new Thread(threadStart).Start();

    }

    //This function is called in a sperate thread and just generates the meshData using the terrainData object, and then adds the data
    //to the queue to be accessed from the main thread
    private void meshDataThread(MapData mapData, int levelOfDetail, Action<MeshData> callBack) {

        MeshData meshData = MeshGenerator.generateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, levelOfDetail, terrainData.useFlatShading);
        MapThreadInfo<MeshData> meshThreadInfo = new MapThreadInfo<MeshData>(callBack, meshData);
        //Prevent multiple threads from accessing it at the same time
        lock(meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(meshThreadInfo);
        }
    }

    //This function is run every physics update, and in it we collect any generated mapData and meshData from the other thread and use it's
    //data in the map
    private void FixedUpdate() {

        //If there is data in the mapData thread queue, get all of it
        if(mapDataThreadInfoQueue.Count > 0) {
            //iterate over all the data
            for(int i = 0; i < mapDataThreadInfoQueue.Count; i++) {
                //get the data
                MapThreadInfo<MapData> mapThreadInfo = mapDataThreadInfoQueue.Dequeue();
                //call the function tht was stored in the thread data with the variable that was computed in the other thread as a parameter, 
                //the supplied callBack Action must only take in one parameter of the correct type for this to work
                mapThreadInfo.callBack(mapThreadInfo.parameter);

            }
        }

        //Do the exact same again for meshData
        if(meshDataThreadInfoQueue.Count > 0) {
            for(int i = 0; i < meshDataThreadInfoQueue.Count; i++) {

                MapThreadInfo<MeshData> meshThreadInfo = meshDataThreadInfoQueue.Dequeue();
                meshThreadInfo.callBack(meshThreadInfo.parameter);

            }
            textureData.updateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);
        }

        //textureData.updateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

    }

    //This function generates the required hightMap for the mesh at the given coordinate
    private MapData generateMapData(Vector2 centre) {

        //Generate the height map from the noiseData values, we have an extra vertex on each side of the map that isn't rendered so that the mesh's edges,
        //will face the correct orientation and line up with each other
        float[,] noiseMap = ProceduralNoise.generateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.noiseScale, noiseData.seed, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

        //If we want to use a falloff map we subtract the given falloff value from the height value for each coordinate
        if(terrainData.useFalloffMap) {

            falloffMap = FalloffGenerator.generateFalloffMap(mapChunkSize + 2, falloffCurveA, falloffCurveB);

            //Iterate over all coordinates in the map and take away the falloff value from the height value
            for(int y = 0; y < mapChunkSize + 2; y++) {
                for(int x = 0; x < mapChunkSize + 2; x++) {
                    //All hights are between [0, 1] so we calmp the values to this range
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);

                }
            }

        }
        //return the heightMap stored within the mapData variable
        return new MapData(noiseMap);

    }

    //OnValidate is called whenever the editor values are changed, thus we can regulate all the values whenever they are manipulated
    private void OnValidate() {

        //If there is a terrainData/noiseData/textureData object, subscribe our onValuesUpdated() function to their
        //onValuesUpdated Action so that he function is called whenver a value updates
        if(terrainData != null) {
            //We only want the function to be subscribed once, so we unsubscribe it if it is subscribed already
            terrainData.onValuesUpdated -= onValuesUpdated;
            terrainData.onValuesUpdated += onValuesUpdated;
        }
        //Same again for both noiseData & textureData
        if(noiseData != null) {
            noiseData.onValuesUpdated -= onValuesUpdated;
            noiseData.onValuesUpdated += onValuesUpdated;
        }
        if(textureData != null) {
            textureData.onValuesUpdated -= onTextureValuesUpdated;
            textureData.onValuesUpdated += onTextureValuesUpdated;
        }

    }

}

