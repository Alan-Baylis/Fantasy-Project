using System;
using System.Collections.Generic;
using UnityEngine;

//World stores a refenece to all loaded and unloaded chunks in the game and controls the map in general
public class World : WorldObject {

    //The name of the world, will come in useful when it somes to loading and unloading world changes from a file
    public string worldName;
    //The Game Manager of the game sassion
    public GameManager gameManager;

    //An array of all loaded chunks in the world
    public List<Chunk> loadedChunks;
    //An array of all chunks that are currently being rendered, since it's easier to toggle rendering a chunk than to
    //constantly load and unload chunks, there will always be a buffer of loaded but unrendered chunks around the border of
    //the world
    public List<Chunk> renderedChunks;

    //A list of currently unused chunk objects that can be repurposed in another location to save on loading and
    //unloading sprites and objects in game
    public List<Chunk> unusedChunks;

    //Generate the map at start up
    private void Start() {

        gameManager.getEventsManager().registerEventListener(new MapLoadingHandler(gameManager));
        generateMap();

    }

    public void initialise(Location location, string worldName) {
        this.worldName = worldName;
        location.setWorld(this);
        base.initialise(location);
    }

    //This function is only temporary as the map will not always be a hills biome,
    //it does however show how one goes about generating terrain
    public void generateMap() {

        //Create a new reference to the hills bimoe which stores data on what blocks need to be generated and how
        Biome hills = new Hills();

        //Create a new terrain generator for the hills biome in this world
        TerrainGenerator terrainGenerator = new TerrainGenerator(gameManager, this, hills);

        //Iterate over a 4*4 chunk region to generate 16 chunks
        for(int i = -2; i < 2; i++) {
            for(int j = -2; j < 2; j++) {
                //Get the location of the current chunk
                ChunkLocation chunkLocation = new ChunkLocation(this, i, 0, j);

                Chunk chunk = loadChunk(terrainGenerator, chunkLocation);
                //renderChunk(chunk);
            }
        }

        MapDisplay1 display = FindObjectOfType<MapDisplay1>();
        display.drawMap(4, 4, getLoadedChunks());

    }

    //Get all currently loaded chunks as an array
    public Chunk[] getLoadedChunks() {
        return this.loadedChunks.ToArray();
    }

    //Get all currently rendered chunks as an array
    public Chunk[] getRenderedChunks() {
        return this.renderedChunks.ToArray();
    }

    public bool containsChunk(Location location) {

        ChunkLocation chunkLocation = ChunkLocation.asChunkLocation(location);

        foreach(Chunk chunk in loadedChunks) {
            if(chunk.getLocation() == chunkLocation) {
                return true;
            }
        }
        return false;
    }

    public Chunk getChunk(Location location) {

        ChunkLocation chunkLocation = ChunkLocation.asChunkLocation(location);

        foreach(Chunk chunk in loadedChunks) {
            if(chunk.getLocation() == chunkLocation) {
                return chunk;
            }
        }
        throw new NullReferenceException("No chunk is loaded at the given location");
    }

    public override string ToString() {
        return worldName;
    }

    public Chunk loadChunk(TerrainGenerator terrainGenerator, ChunkLocation chunkLocation) {
        Chunk chunk = null;

        if(unusedChunks.Count > 0) {
            chunk = unusedChunks[0];
            unusedChunks.RemoveAt(0);
            chunk = terrainGenerator.regenerateChunk(chunk, chunkLocation, Direction.NORTH);
        } else {

            //Populate all columns in the chunk using the data in the terrain generator
            chunk = terrainGenerator.generateChunk(chunkLocation, Direction.NORTH);
            //set the chunk to be nested under the world in the heirarchy
            chunk.transform.SetParent(transform);
        }
        //Add this chunk to the list of loaded chunks
        loadedChunks.Add(chunk);

        return chunk;
    }

    public void renderChunk(Chunk chunk) {

        if(chunk.isRendered()) {
            return;
        }

        //render the chunk
        chunk.toggleRender();
        renderedChunks.Add(chunk);

    }

    public void destroyChunk(Chunk chunk) {

        foreach(Transform child in chunk.transform) {
            //GameObject.Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
        //GameObject.Destroy(chunk.transform.gameObject);
        chunk.transform.gameObject.SetActive(false);

        if(loadedChunks.Contains(chunk)) {
            loadedChunks.Remove(chunk);
        }
        if(renderedChunks.Contains(chunk)) {
            renderedChunks.Remove(chunk);
        }
        unusedChunks.Add(chunk);

    }

}