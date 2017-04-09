using System.Collections;
using UnityEngine;

//This class is the machinery behind all terrain generation that uses the data stored in the biome and GeneratorData
//to create terrain in the world
//it generates terrain on a column by column basis 
public class TerrainGenerator {

    private GameManager gameManager;

    //The biome stores information about waht blocks need to be generated
    private Biome biome;
    //The world for which the terrain will be generated
    private World world;

    //Constructor...
    public TerrainGenerator(GameManager gameManager, World world, Biome biome) {

        this.gameManager = gameManager;
        this.biome = biome;
        this.world = world;

    }

    //Get the biome
    public Biome getBiome() {
        return this.biome;
    }

    //Get the world
    public World getWorld() {
        return this.world;
    }

    //Generate a chunk at the given location, it populates all chunk columns in the chunk with 
    //block type height maps
    public Chunk generateChunk(ChunkLocation chunkLocation, Direction direction) {

        //get the biome and the world the terrain is being generated for
        Biome biome = getBiome();
        World world = chunkLocation.getWorld();

        //Instantiate the GameObject in the gme that represents a chunk from file
        GameObject chunkGameObject = MonoBehaviour.Instantiate(Resources.Load("Chunk", typeof(GameObject))) as GameObject;
        //Get the chunk script attached to this GameObject
        Chunk chunk = chunkGameObject.GetComponent("Chunk") as Chunk;
        //Set up the values in the chunk script
        chunk.initialise(chunkLocation);

        //Get the x and y coordinates of the chunk
        int chunkX = chunk.getLocation().getBlockX();
        int chunkZ = chunk.getLocation().getBlockZ();

        //Iterate over all columns in the chunk
        for(int x = 0; x < Chunk.chunkSize; x++) {
            for(int z = 0; z < Chunk.chunkSize; z++) {

                //Get the coordinate of the column as the chunk's location in the world and the column's position in the
                //chunk, it's z-coord (height) can be 0
                Location columnBaseLocation = new Location(world, chunkX + x, 0, chunkZ + z);

                //Instantieate a ChunkColumn GameObject from file aswell
                GameObject columnGameObject = MonoBehaviour.Instantiate(Resources.Load("Chunk Column", typeof(GameObject))) as GameObject;
                //Set the chunk column to be a child of the chunk in the heirarchy
                columnGameObject.transform.SetParent(chunkGameObject.transform);
                //Get the ChunkColumn script attached to this GameObject
                ChunkColumn chunkColumn = columnGameObject.GetComponent("ChunkColumn") as ChunkColumn;
                //initialise the variables in the chunkcolumn
                chunkColumn.initialise(chunk, biome, columnBaseLocation, direction);

                //Fill the column with blocks based off of the biome
                gameManager.StartCoroutine("populateColumn", chunkColumn);
                //Generate caves in the column (air spaces)
                gameManager.StartCoroutine("generateCaves", chunkColumn);

                //Set the chunk column in the parent chunk to be this chunk column
                chunk.setColumn(columnBaseLocation, chunkColumn);

            }
        }

        //Return the newly generated chunk
        return chunk;
    }

    //Generate a chunk at the given location, it populates all chunk columns in the chunk with 
    //block type height maps
    public Chunk regenerateChunk(Chunk chunk, ChunkLocation chunkLocation, Direction direction) {

        //get the biome and the world the terrain is being generated for
        Biome biome = getBiome();
        World world = chunkLocation.getWorld();
        //Get the GameObject of the chunk
        GameObject chunkGameObject = chunk.gameObject;
        chunkGameObject.SetActive(true);

        //Set up the values in the chunk script
        chunk.initialise(chunkLocation);

        //Get the x and y coordinates of the chunk
        int chunkX = chunk.getLocation().getBlockX();
        int chunkZ = chunk.getLocation().getBlockZ();

        int i = 0;
        //Iterate over all columns in the chunk
        for(int x = 0; x < Chunk.chunkSize; x++) {
            for(int z = 0; z < Chunk.chunkSize; z++) {

                //Get the coordinate of the column as the chunk's location in the world and the column's position in the
                //chunk, it's z-coord (height) can be 0
                Location columnBaseLocation = new Location(world, chunkX + x, 0, chunkZ + z);

                //Debug.Log(i);
                ChunkColumn chunkColumn = chunk.getColumns()[i];
                i++;
                chunkColumn.gameObject.SetActive(true);

                chunkColumn.initialise(chunk, biome, columnBaseLocation, direction);

                //Fill the column with blocks based off of the biome
                gameManager.StartCoroutine("populateColumn", chunkColumn);
                //Generate caves in the column (air spaces)
                gameManager.StartCoroutine("generateCaves", chunkColumn);

                //Set the chunk column in the parent chunk to be this chunk column
                chunk.setColumn(columnBaseLocation, chunkColumn);

            }
        }

        //Return the newly generated chunk
        return chunk;
    }
}
