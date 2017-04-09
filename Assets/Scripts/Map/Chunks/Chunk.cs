using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A chunk inherits from WorldObject since it can be instantiated within the game, 
//A chunk stores a 16*16 square of chunk columns 
public class Chunk : WorldObject {

    //The number of blocks per length and width of the chunk,
    //this number squared is the number of columns per chunk
    public static int chunkSize = 16;

    //The location of the chunk in the world
    private ChunkLocation location;
    //Whether or not all the chunk columns in the chunk need to be rendered
    private bool rendered = false;

    //A list of all the locations of the chunk columns in the chunk
    public List<Location> columnBaseLocations = new List<Location>();
    //A dictionary that references the chunk column at the given chunk column location
    public Dictionary<Location, ChunkColumn> columns = new Dictionary<Location, ChunkColumn>();

    //intialise sets up all the necessary variables whn the chunk is instantiated
    public void initialise(ChunkLocation location) {
        //Set up the location in WorldObject
        base.initialise(location);
        //Set the position of the chunk in the game to it's location object
        transform.position = location.getPosition();
    }

    //Change the rendering of all chunkcolumns in the chunk, if it's already being rendered, stop rendering and vice versa
    public void toggleRender() {

        //Toggle the rendering in all the chunk columns of the chunk
        foreach(ChunkColumn chunkColumn in getColumns()) {
            chunkColumn.toggleRender();
        }
        //Toggle the variable that tracks whether the chunk is being rendered
        this.rendered = !isRendered();

    }

    //Whether or not all chunk columns in the chunk are rendered or not
    public bool isRendered() {
        return this.rendered;
    }

    //Get all the columns in the chunk as an array
    public ChunkColumn[] getColumns() {
        //Create a new empty array with the dimensions of the chunk
        ChunkColumn[] chunkColumns = new ChunkColumn[Chunk.chunkSize * Chunk.chunkSize];
        //Iterate over all chunk columns in the chunk and add them to the array
        int i = 0;
        foreach(ChunkColumn chunkColumn in columns.Values) {
            chunkColumns[i] = chunkColumn;
            i++;
        }

        return chunkColumns;

    }

    //Replace an existing chunk column at the specified entry with a new one
    public void setColumn(Location location, ChunkColumn chunkColumn) {

        //Remove the old entry if there is one and then add the new entry
        if(columns.ContainsKey(location)) {
            columns.Remove(location);
            columnBaseLocations.Remove(location);
        }
        //Add the new chunk column both to the dictionary and the list of locations
        columns.Add(location, chunkColumn);
        columnBaseLocations.Add(location);

    }

    //Get the chunk column at the given location if it exists
    public ChunkColumn getColumn(Location location) {

        int x = location.getBlockX();
        int z = location.getBlockZ();
        return getColumn(x, z);

    }

    public ChunkColumn getColumn(int x, int z) {

        foreach(ChunkColumn chunkColumn in getColumns()) {

            int columnX = chunkColumn.getLocation().getBlockX();
            int columnZ = chunkColumn.getLocation().getBlockZ();

            if(x == columnX && z == columnZ) {
                return chunkColumn;
            }

        }
        return null;
    }

}
