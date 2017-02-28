using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    //The number of blocks per length and width of the chunk
    public static int chunkSize = 16;

    private Location location;
    public Block[,] blocks;
    public Dictionary<Location, ChunkColumn> columns = new Dictionary<Location, ChunkColumn>();

    public Chunk(Location location, Biome biome) {
        this.location = location;
    }

    public Location getLocation() {
        return this.location;
    }

    public void setColumn(Location location, ChunkColumn chunkColumn) {

        //Remove the old entry if there is one and then add the new entry
        if(columns.ContainsKey(location)) { 
            columns.Remove(location);
        }
        columns.Add(location, chunkColumn);

    }

    private void Start() {

    }

}
