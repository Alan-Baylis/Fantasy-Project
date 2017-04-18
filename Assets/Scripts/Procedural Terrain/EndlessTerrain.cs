using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour {

    public const float playerMoveThresholdForChunkUpdate = 25f;
    public const float squarePlayerMoveThreholdForChunkUpdate = playerMoveThresholdForChunkUpdate * playerMoveThresholdForChunkUpdate;

    public LODInfo[] detailLevels;
    public static float maxViewDistance;

    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 playerPosition;
    private Vector2 previousPlayerPosition;
    public static MapGenerator mapGenerator;

    int chunkSize;
    int chunksVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDicttionary = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start() {

        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;

        chunkSize = mapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);

        //Draw the chunks on start
        updateVisibleChunks();

    }

    private void Update() {

        playerPosition = new Vector2(viewer.position.x, viewer.position.z) / mapGenerator.terrainData.uniformScale;

        if((previousPlayerPosition - playerPosition).sqrMagnitude > squarePlayerMoveThreholdForChunkUpdate) {
            previousPlayerPosition = playerPosition;
            updateVisibleChunks();
        }

    }

    public void updateVisibleChunks() {

        foreach(TerrainChunk terrainChunk in terrainChunksVisibleLastUpdate) {
            terrainChunk.setVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int currentChunkY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        for(int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++) {
            for(int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++) {

                Vector2 viewedChunkCoord = new Vector2(currentChunkX + xOffset, currentChunkY + yOffset);

                if(terrainChunkDicttionary.ContainsKey(viewedChunkCoord)) {

                    TerrainChunk terrainChunk = terrainChunkDicttionary[viewedChunkCoord];
                    terrainChunk.updateTerrainChunk();

                } else {
                    terrainChunkDicttionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, mapMaterial, transform));
                }

            }
        }

    }


    public class TerrainChunk {

        private Vector2 position;
        private GameObject meshGameObject;
        private Bounds bounds;

        private MeshRenderer meshRenderer;
        private MeshFilter meshFilter;
        private MeshCollider meshCollider;

        private LODInfo[] detailLevels;
        private LODMesh[] lodMeshes;
        private LODMesh collisionLODMesh;

        private MapData mapData;
        private bool mapDataReceived;

        private int previousLODIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Material material, Transform parent) {

            this.detailLevels = detailLevels;

            this.position = coord * size;
            Vector3 position3D = new Vector3(position.x, 0, position.y);

            this.bounds = new Bounds(position, Vector3.one * size);

            this.meshGameObject = new GameObject("Terrain Chunk");
            this.meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
            this.meshFilter = meshGameObject.AddComponent<MeshFilter>();
            this.meshCollider = meshGameObject.AddComponent<MeshCollider>();

            meshRenderer.material = material;

            meshGameObject.transform.position = position3D * mapGenerator.terrainData.uniformScale;
            meshGameObject.transform.parent = parent;
            meshGameObject.transform.localScale = Vector3.one * mapGenerator.terrainData.uniformScale;

            setVisible(false);

            this.lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i < detailLevels.Length; i++) {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, updateTerrainChunk);
                if(detailLevels[i].useForCollider) {
                    collisionLODMesh = lodMeshes[i];
                }
            }

            mapGenerator.requestMapData(position, onMapDataReceived);

        }

        public void updateTerrainChunk() {

            if(!mapDataReceived) {
                return;
            }

            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

            setVisible(visible);

            if(visible) {

                int lodIndex = 0;
                for(int i = 0; i < detailLevels.Length - 1; i++) {

                    if(viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold) {
                        lodIndex = i + 1;
                    } else {
                        break;
                    }

                }

                if(lodIndex != previousLODIndex) {

                    LODMesh lodMesh = lodMeshes[lodIndex];
                    if(lodMesh.hasReceivedMesh) {

                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;

                    } else if(!lodMesh.hasRequestedMesh) {

                        lodMesh.requestMesh(mapData);

                    }

                }

                if(lodIndex == 0) {
                    if(collisionLODMesh.hasReceivedMesh) {
                        meshCollider.sharedMesh = collisionLODMesh.mesh;
                    } else if(!collisionLODMesh.hasRequestedMesh) {
                        collisionLODMesh.requestMesh(mapData);
                    }
                }

                terrainChunksVisibleLastUpdate.Add(this);

            }

        }

        public void onMapDataReceived(MapData mapData) {

            this.mapData = mapData;
            mapDataReceived = true;

            updateTerrainChunk();

        }

        public void setVisible(bool visible) {
            meshGameObject.SetActive(visible);
        }

        public bool isVisible() {
            return meshGameObject.activeSelf;
        }

    }

    //LOD = Level Of Detail
    class LODMesh {

        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasReceivedMesh;
        private int lod;

        private Action updateCallBack;

        public LODMesh(int lod, Action updateCallBack) {
            this.lod = lod;
            this.updateCallBack = updateCallBack;
        }

        public void requestMesh(MapData mapData) {

            hasRequestedMesh = true;
            mapGenerator.requestMeshData(mapData, lod, onMeshDataReceived);

        }

        void onMeshDataReceived(MeshData meshData) {

            mesh = meshData.createMesh();
            hasReceivedMesh = true;

            updateCallBack();

        }

    }

    [Serializable]
    public struct LODInfo {

        public int lod;
        public float visibleDistanceThreshold;
        public bool useForCollider;

    }

}
