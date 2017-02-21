using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//The super structure that controls all chunks in the game
public class World : MonoBehaviour {

    //A dictionary to store a reference to all loaded chunks by their position in the world
    public Dictionary<WorldPosition, Chunk> chunks = new Dictionary<WorldPosition, Chunk>();
    //A reference to the GameObject that represents a chunk
    public GameObject chunkPrefab;
    //Well...you guessed it.
    public string worldName = "world";

    //Load a chunk into the world at the given coordinate
    public void CreateChunk(int x, int y, int z) {
        //Create a reference to the coordinate as a WorldPosition, for the dictionary
        WorldPosition WorldPosition = new WorldPosition(x, y, z);

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(
                        chunkPrefab, WorldPosition.asTransform(),
                        //Sets it's rotation to 0
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

        //Get a reference to the chunk script from the GameObject
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();
        //Set the position to the WorldPosition
        newChunk.pos = WorldPosition;
        //Assign the world to be this script
        newChunk.world = this;

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(WorldPosition, newChunk);
        //Make a reference to a new Terrain Generator
        var TerrainGenerator = new TerrainGenerator();
        //Generate the blocks in the chunk
        newChunk = TerrainGenerator.ChunkGen(newChunk);
        //Make it so that the blocks are not referenced as being modified as they are just created, they are marked as modified if they 
        //are changed by the player.
        newChunk.SetBlocksUnmodified();
        //The Serialization class handles loading and saving the chunks to a file
        Serialization.Load(newChunk);
    }

    //A function that removes a chunk from the scene to unrender it
    public void destroyChunk(WorldPosition worldPosition) {
        Chunk chunk = null;
        //Try to get the chunk from the chunks dictionary based off of the WorldPosition key, it assigns any value it finds to 
        //chunk and then returns it.
        if(chunks.TryGetValue(worldPosition, out chunk)) {
            //Save the chunk to file before removing it.
            Serialization.SaveChunk(chunk);
            //Destroy the actual GameObject
            Object.Destroy(chunk.gameObject);
            //And remove it from the dictionary
            chunks.Remove(worldPosition);
        }
    }

    //A function that removes a chunk from the scene to unrender it, this just calls the above function
    public void destroyChunk(int x, int y, int z) {
        WorldPosition position = new WorldPosition(x, y, z);
        destroyChunk(position);
    }

    //Returns the chunk at the given coordinate if it exists and if the coordinate is valid
    public Chunk getChunk(WorldPosition worldPosition) {
        //Initialise a null chunk
        Chunk containerChunk = null;
        //Try to assign it's value from the dictionary
        chunks.TryGetValue(worldPosition, out containerChunk);
        //return...
        return containerChunk;

    }

    public Chunk getChunk(int x, int y, int z) {
        //Convert the chunk size to a float so the division produces a float not an int
        float multiple = Chunk.chunkSize;
        //Floor the division of all the coordinates to chunk coordinates and then multiply by the size of each chunk:
        //The division effectively gives what number chunk it is from the origin along the respective axis 
        //Multiplying by chunkSize makes it a coordinate for the chunk
        x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;
        //Make a world position
        WorldPosition position = new WorldPosition(x, y, z);
        //Call the other getChunk() function
        return getChunk(position);
    }

    //Returns the block at the position
    public Block getBlock(int x, int y, int z) {
        //Get the chunk the block is in
        Chunk containerChunk = getChunk(x, y, z);
        //If the chunk exists:
        if(containerChunk != null) {
            //Get the block at relative coordinates within the chunk, so take away the chunk coordinates to get relative coordinates.
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        } else {
            //If it doesn't exist, the block defaults to AIR
            return new BlockAir();
        }

    }

    //Set the block at the given coordinate to the supplied block type
    public void setBlock(int x, int y, int z, Block block) {
        //Get the containing chunk
        Chunk chunk = getChunk(x, y, z);
        //If the chunk exists:
        if(chunk != null) {
            //Set the block in the chunk to the type, using relative coordinates within the chunk again
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            //Tell the chunk it needs to update
            chunk.update = true;
            //This gets the neighbouring chunks to update if the setBlock is at the edge of the chunk and neighbouring blocks
            //are in another chunk
            //Check one up and down on the x axis
            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPosition(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPosition(x + 1, y, z));
            //Compare one block up and down on the y axis
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPosition(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPosition(x, y + 1, z));
            //Check one left and right on the z axis
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPosition(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPosition(x, y, z + 1));

        }
    }

    //This function compares the 1st 2 values, if they are equal, it updates the chunk at the given world position
    //It is used to update neighbouring chunks for blocks at chunk boundaries
    void UpdateIfEqual(int value1, int value2, WorldPosition pos) {
        if(value1 == value2) {
            //Update the chunk at the given cooredinate if it exists
            Chunk chunk = getChunk(pos);
            if(chunk != null)
                chunk.update = true;
        }
    }
}
