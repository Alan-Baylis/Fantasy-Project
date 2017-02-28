using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkColumn {

    public static int blockMapHeight = 256;

    private List<Block> column = new List<Block>();
    private Location columnBaseLocation;

    public ChunkColumn(Location columnBaseLocation) {

        int x = columnBaseLocation.getX();
        int y = columnBaseLocation.getY();

        this.columnBaseLocation = columnBaseLocation;
        for(int z = 0; z < blockMapHeight; z++) {
            Location blockLocation = new Location(x, y, z);
            column.Add(new Air(blockLocation));
        }

    }

    public Location getColumnBaseLocation() {
        return this.columnBaseLocation;
    }

    public Block[] getColumn() {
        return column.ToArray();
    }

    public void setBlock(int z, Block block) {
        column.Insert(z, block);
    }
	
}
