using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode {
        NOISEMAP,
        MESH,
        FALLOFFMAP
    };
    public DrawMode drawMode;

    public bool autoUpdate;

    public TerrainData terrainData;
    public NoiseData noiseData;
    public TextureData textureData;

    public Material terrainMaterial;

    public int mapChunkSize {
        get {
            if(terrainData.useFlatShading) {
                return 95;
            } else {
                return 239;
            }
        }
    }

    [Range(0, 3)]
    public int editorLOD;

    Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    private float[,] falloffMap;
    public float falloffCurveA;
    public float falloffCurveB;

    private void onValuesUpdated() {

        if(!Application.isPlaying) {
            drawMapInEditor();
        }

    }

    private void onTextureValuesUpdated() {
        textureData.applyToMaterial(terrainMaterial);
    }

    public void drawMapInEditor() {

        MapData mapData = generateMapData(Vector2.zero);
        textureData.updateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();

        if(drawMode == DrawMode.NOISEMAP) {

            Texture2D texture = TextureGenerator.textureFromHeightMap(mapData.heightMap);
            mapDisplay.drawTexture(texture);

        } else if(drawMode == DrawMode.MESH) {

            MeshData meshData = MeshGenerator.generateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, editorLOD, terrainData.useFlatShading);

            mapDisplay.drawMesh(meshData);

        } else if(drawMode == DrawMode.FALLOFFMAP) {

            Texture2D texture = TextureGenerator.textureFromHeightMap(falloffMap);
            mapDisplay.drawTexture(texture);

        }

    }

    public void requestMapData(Vector2 centre, Action<MapData> callBack) {

        ThreadStart threadStart = delegate {
            mapDataThread(centre, callBack);
        };
        new Thread(threadStart).Start();

    }

    void mapDataThread(Vector2 centre, Action<MapData> callBack) {

        MapData mapData = generateMapData(centre);
        MapThreadInfo<MapData> mapThreadInfo = new MapThreadInfo<MapData>(callBack, mapData);
        //Prevent multiple threads from accessing it at the same time
        lock(mapDataThreadInfoQueue) {
            mapDataThreadInfoQueue.Enqueue(mapThreadInfo);
        }
    }

    public void requestMeshData(MapData mapData, int levelOfDetail, Action<MeshData> callBack) {

        ThreadStart threadStart = delegate {
            meshDataThread(mapData, levelOfDetail, callBack);
        };
        new Thread(threadStart).Start();

    }

    void meshDataThread(MapData mapData, int levelOfDetail, Action<MeshData> callBack) {

        MeshData meshData = MeshGenerator.generateTerrainMesh(mapData.heightMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, levelOfDetail, terrainData.useFlatShading);
        MapThreadInfo<MeshData> meshThreadInfo = new MapThreadInfo<MeshData>(callBack, meshData);
        //Prevent multiple threads from accessing it at the same time
        lock(meshDataThreadInfoQueue) {
            meshDataThreadInfoQueue.Enqueue(meshThreadInfo);
        }
    }

    private void Update() {

        if(mapDataThreadInfoQueue.Count > 0) {
            for(int i = 0; i < mapDataThreadInfoQueue.Count; i++) {

                MapThreadInfo<MapData> mapThreadInfo = mapDataThreadInfoQueue.Dequeue();
                mapThreadInfo.callBack(mapThreadInfo.parameter);

            }
        }

        if(meshDataThreadInfoQueue.Count > 0) {
            for(int i = 0; i < meshDataThreadInfoQueue.Count; i++) {

                MapThreadInfo<MeshData> meshThreadInfo = meshDataThreadInfoQueue.Dequeue();
                meshThreadInfo.callBack(meshThreadInfo.parameter);

            }
        }
        textureData.updateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

    }

    private MapData generateMapData(Vector2 centre) {

        float[,] noiseMap = ProceduralNoise.generateNoiseMap(mapChunkSize + 2, mapChunkSize + 2, noiseData.noiseScale, noiseData.seed, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, centre + noiseData.offset, noiseData.normalizeMode);

        if(terrainData.useFalloffMap) {

            if(falloffMap == null) {
                falloffMap = FalloffGenerator.generateFalloffMap(mapChunkSize + 2, falloffCurveA, falloffCurveB);
            }

            for(int y = 0; y < mapChunkSize + 2; y++) {
                for(int x = 0; x < mapChunkSize + 2; x++) {

                    if(terrainData.useFalloffMap) {
                        noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                    }

                }
            }

        }

        //textureData.updateMeshHeights(terrainMaterial, terrainData.minHeight, terrainData.maxHeight);

        return new MapData(noiseMap);

    }

    private void OnValidate() {

        if(terrainData != null) {
            terrainData.onValuesUpdated -= onValuesUpdated;
            terrainData.onValuesUpdated += onValuesUpdated;
        }
        if(noiseData != null) {
            noiseData.onValuesUpdated -= onValuesUpdated;
            noiseData.onValuesUpdated += onValuesUpdated;
        }
        if(textureData != null) {
            textureData.onValuesUpdated -= onTextureValuesUpdated;
            textureData.onValuesUpdated += onTextureValuesUpdated;
        }

    }

    struct MapThreadInfo<T> {
        public readonly Action<T> callBack;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callBack, T parameter) {
            this.callBack = callBack;
            this.parameter = parameter;
        }
    }

}

public struct MapData {

    public readonly float[,] heightMap;

    public MapData(float[,] heightMap) {

        this.heightMap = heightMap;

    }
}