using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles the loading of the map around the player as they move
public class EndlessTerrain : MonoBehaviour {

    //The distance a player must move before we update the ap, so we aren't constantly loading and unloading 
    public const float playerMoveThresholdForChunkUpdate = 25f;
    //Squared distances are easier to compute as they don't require a square root, hence it's more efficient to use square distances
    public const float squarePlayerMoveThreholdForChunkUpdate = playerMoveThresholdForChunkUpdate * playerMoveThresholdForChunkUpdate;

    //An array of LODInfo that describes the detail level of meshes and how close they have to be to have that detail level
    public LODInfo[] detailLevels;
    //The furthest distance a mesh can be from the player
    public static float maxViewDistance;

    //The Transform component of the player
    public Transform viewer;
    //The material/shader of the map
    public Material mapMaterial;

    //The current position of the player on a 2d plane across the map
    public static Vector2 playerPosition;
    //The position of the player when we last updated the meshes
    private Vector2 previousPlayerPosition;
    //A reference to the mapGenerator script
    public static MapGenerator mapGenerator;

    //The size of each mesh
    int chunkSize;
    //The number of meshes loaded
    int chunksVisibleInViewDistance;

    //Keep a reference to each loaded mesh as it's coordinate in 2d space
    Dictionary<Vector2, TerrainChunk> terrainChunkDicttionary = new Dictionary<Vector2, TerrainChunk>();
    //A list of all currently loaded chunks
    static List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start() {

        //Get a reference to the mapGenerator script on load up
        mapGenerator = FindObjectOfType<MapGenerator>();

        //The LODInfo array is ordered with the furthest away meshes having the lowest lod and being at the end of the array, so the max view distance is the smallest lod we can have in range
        maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDistanceThreshold;

        //The map chunk size found in mapGenerator includes the excess vertices so we can ignore them
        chunkSize = mapGenerator.mapChunkSize - 1;
        //The number of chunks is the the amount we can fit into the max distance
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);

        //Draw the chunks on start
        updateVisibleChunks();

    }

    private void Update() {
        //Get the player's position every time they move, and scale it to be the same scale as the map
        playerPosition = new Vector2(viewer.position.x, viewer.position.z) / mapGenerator.terrainData.uniformScale;

        //Is the player has moved further than the update threshold distance
        if((previousPlayerPosition - playerPosition).sqrMagnitude > squarePlayerMoveThreholdForChunkUpdate) {
            //reassign the position at which we last updated the terrain
            previousPlayerPosition = playerPosition;
            //update the terrain
            updateVisibleChunks();
        }

    }

    public void updateVisibleChunks() {

        //Iterate over all current meshes visible in the game and make them invisible
        foreach(TerrainChunk terrainChunk in terrainChunksVisibleLastUpdate) {
            terrainChunk.setVisible(false);
        }
        //Clear the list of all meshes that were visible last update
        terrainChunksVisibleLastUpdate.Clear();

        //Get the current chunk that the player is in
        int currentChunkX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int currentChunkY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        //Iterate over all meshes behind the player to the ones in front of them in all directions
        for(int yOffset = -chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++) {
            for(int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++) {

                //All chunks are centered around the current one the player is in, so we get all possible ones in the max distance around it
                Vector2 viewedChunkCoord = new Vector2(currentChunkX + xOffset, currentChunkY + yOffset);

                //If there is already a mesh at this chunk coord
                if(terrainChunkDicttionary.ContainsKey(viewedChunkCoord)) {
                    //Get the terrain chunk reference for this chunk coord
                    TerrainChunk terrainChunk = terrainChunkDicttionary[viewedChunkCoord];
                    //update the lod of the mesh
                    terrainChunk.updateTerrainChunk();

                } else {
                    //otherwise this chunk has never been loaded before and we make a new one, and then enter it as a refernce in the dictionary
                    terrainChunkDicttionary.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, mapMaterial, transform));
                }

            }
        }

    }

    //This class represents each chunk in the game and stores data pertaining to it
    public class TerrainChunk {

        //The 2d position of the chunk in the game as referenced in the dictionary above
        private Vector2 position;
        //The gameObject of the mesh that represents the chun in game
        private GameObject meshGameObject;

        //Bounds represents a bounding box along the axis and is used to orient all vector distances
        private Bounds bounds;

        //A reference to the image renderer on the mesh
        private MeshRenderer meshRenderer;
        //A reference to the mesh shape
        private MeshFilter meshFilter;
        //Reference to the collider on the mesh
        private MeshCollider meshCollider;

        //The LODInfo like above
        private LODInfo[] detailLevels;
        //A reference to the mesh as rendered with the specific lod from each lodinfo entry
        private LODMesh[] lodMeshes;
        //A reference to the collision mesh data and the lod that a mesh must have in order to have a collider
        private LODMesh collisionLODMesh;

        //The mapData (noise map) for this chunk
        private MapData mapData;
        //Whether or not the map data has finished being generated in a seperate thread
        private bool mapDataReceived;

        //Start the previous lod index at -1, since it is impossible so it is guaranted to be atleast initially updated
        private int previousLODIndex = -1;

        //The constructor takes in the position of it in the game, it's dimensions as a square, the possible Lod detail levels for future reference, the material to be applied to the mesh for rendering
        //And the gameObject that it is going to be nested under in the editor hierarchy
        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, Material material, Transform parent) {

            //Assign the LODInfo array for future reference
            this.detailLevels = detailLevels;

            //save it's position in the game
            this.position = coord * size;

            //Get it's position in 3d space used for crating the game object mesh later 
            Vector3 position3D = new Vector3(position.x, 0, position.y);

            //Create a new bounding box for the size of the chunk 
            this.bounds = new Bounds(position, Vector3.one * size);

            //Generate the empty mesh gameObject
            this.meshGameObject = new GameObject("Terrain Chunk");
            //Add the required components to the mesh GameObject
            this.meshRenderer = meshGameObject.AddComponent<MeshRenderer>();
            this.meshFilter = meshGameObject.AddComponent<MeshFilter>();
            this.meshCollider = meshGameObject.AddComponent<MeshCollider>();

            //assign the rendering material/shader to the mesh
            meshRenderer.material = material;

            //Set the position of the gameObject
            meshGameObject.transform.position = position3D * mapGenerator.terrainData.uniformScale;
            //Assign its position in the heirarchy
            meshGameObject.transform.parent = parent;
            //Scale up the chunk to the map scale
            meshGameObject.transform.localScale = Vector3.one * mapGenerator.terrainData.uniformScale;

            //Make it invisible to begin with
            setVisible(false);

            //Create a new empty array of all basic meshes for each lod, there is one mesh for each lod
            this.lodMeshes = new LODMesh[detailLevels.Length];

            //Iterate over every lod
            for(int i = 0; i < detailLevels.Length; i++) {
                //Assign a new LodMesh struct to each entry in the array, with the updateTerrainChunk() function to be called once the mesh is fully generated
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, updateTerrainChunk);
                //If the lod of this mesh is the od we wan to use for a collider, save a reference for the collider LODMesh aswell
                if(detailLevels[i].useForCollider) {
                    collisionLODMesh = lodMeshes[i];
                }
            }

            //Start generating the mapData in another thread for the chunk at it's position and call the function onMapDataReceived() when finished
            mapGenerator.requestMapData(position, onMapDataReceived);

        }

        //This function is called once the mapData for ths chunk has been fully generated in another thread
        public void onMapDataReceived(MapData mapData) {

            //Save a reference to the generated mapData
            this.mapData = mapData;
            //we have now received the mapData
            mapDataReceived = true;
            //Update the terrain chunk with the mapData that has been received
            updateTerrainChunk();

        }

        //This function handles the switching of lod for the mesh of the chunk as the player moves around
        public void updateTerrainChunk() {

            //We can't update the terrain if we don't have any mapData to work off
            if(!mapDataReceived) {
                return;
            }
            //Get the distance of the player from the bounding box of the chunk
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            //The chunk is visible if the chunk is not too far away from the player
            bool visible = viewerDistanceFromNearestEdge <= maxViewDistance;

            //Change the visibility of the chunk based off of whether it is in range
            setVisible(visible);

            //If the chunk is in range of the player, then we need to figure out what it's mesh looks like
            if(visible) {

                //Start out assuming that we are at the highest detail level, then iterate over all subsequent detail levels, until we find the correct distance that reflects our lod
                int lodIndex = 0;
                for(int i = 0; i < detailLevels.Length - 1; i++) {

                    //We use the lod for which our distance is less than the given distance for the lod
                    if(viewerDistanceFromNearestEdge > detailLevels[i].visibleDistanceThreshold) {
                        //So if we are too far away we increment our lod
                        lodIndex = i + 1;
                    } else {
                        //otherwise we are in range and we can stop iterating over the lods
                        break;
                    }

                }

                //If our lod index has changed since the player moved last
                if(lodIndex != previousLODIndex) {
                    //Get the lod for this index
                    LODMesh lodMesh = lodMeshes[lodIndex];
                    //If there is a mesh for this given lod:
                    if(lodMesh.hasReceivedMesh) {
                        //Reassign our current index of lod
                        previousLODIndex = lodIndex;
                        //Assign the mesh from the lodMesh
                        meshFilter.mesh = lodMesh.mesh;
                    //Otherwise if the mesh hasn't received a mesh it could just be aiting for it to be generated in another thread, however if the mesh generation hasn't even
                    //been requested, then we get it to request a mesh
                    } else if(!lodMesh.hasRequestedMesh) {

                        lodMesh.requestMesh(mapData, mapGenerator);

                    }

                }
                //If we are at the highest lod
                if(lodIndex == 0) {
                    //If the mesh collider has been generated, we add a collider to the mesh
                    if(collisionLODMesh.hasReceivedMesh) {
                        meshCollider.sharedMesh = collisionLODMesh.mesh;
                        //Otherwise we get the collider to generate it's mesh 
                    } else if(!collisionLODMesh.hasRequestedMesh) {
                        collisionLODMesh.requestMesh(mapData, mapGenerator);
                    }
                }
                //Add this terrain chunk to the list of all chunks that were visible last update
                terrainChunksVisibleLastUpdate.Add(this);

            }

        }

        //The following two functions simplify the checking of whether or not the mesh GameObject is visible and changing it's visibilty
        public void setVisible(bool visible) {
            meshGameObject.SetActive(visible);
        }

        public bool isVisible() {
            return meshGameObject.activeSelf;
        }

    }

}
