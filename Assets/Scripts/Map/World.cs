using System.Collections.Generic;
using UnityEngine;

//World stores a refenece to all loaded and unloaded chunks in the game and controls the map in general
public class World : WorldObject {

    //The name of the world, will come in useful when it somes to loading and unloading world changes from a file
    public string worldName;

    //An array of all loaded chunks in the world
    public List<Chunk> loadedChunks;
    //An array of all chunks that are currently being rendered, since it's easier to toggle rendering a chunk than to
    //constantly load and unload chunks, there will always be a buffer of loaded but unrendered chunks around the border of
    //the world
    public List<Chunk> renderedChunks;

    //Generate the map at start up
    private void Start() {

        generateMap();

    }

    //This function is only temporary as the map will not always be a hills biome,
    //it does however show how one goes about generating terrain
    public void generateMap() {

        //Create a new reference to the hills bimoe which stores data on what blocks need to be generated and how
        Biome hills = new Hills();

        //Create a new terrain generator for the hills biome in this world
        TerrainGenerator terrainGenerator = new TerrainGenerator(this, hills);

        //Iterate over a 4*4 chunk region to generate 16 chunks
        for(int i = 0; i < 4; i++) {
            for(int j = 0; j < 4; j++) {
                //Get the location of the current chunk
                ChunkLocation chunkLocation = new ChunkLocation(this, i, j, 0);
                //Populate all columns in the chunk using the data in the terrain generator
                Chunk chunk = terrainGenerator.generateChunk(chunkLocation, Direction.NORTH);
                //set the chunk to be nested under the world in the heirarchy
                chunk.transform.parent = transform.parent;

                //Add this chunk to the list of loaded chunks
                loadedChunks.Add(chunk);
                //render the chunk
                chunk.toggleRender();
                renderedChunks.Add(chunk);
            }
        }
    }

    //Get all currently loaded chunks as an array
    public Chunk[] getLoadedChunks() {
        return this.loadedChunks.ToArray();
    }

    //GEt all currently rendered chunks as an array
    public Chunk[] getRenderedChunks() {
        return this.renderedChunks.ToArray();
    }

    public Chunk getChunk(Location chunkLocation) {

        foreach(Chunk chunk in loadedChunks) {
            if(chunk.getLocation() == chunkLocation) {
                return chunk;
            }
        }
        return null;

    }

}