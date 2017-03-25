using UnityEngine;

//ChunkLocation is a subclass of Location where every inputed coordinate is 16 times bigger than the given coordinate, to 
//account for chunk sizes
 public class ChunkLocation : Location {

    //Initialise the location at the world and the x.y,z coordinates
    public ChunkLocation(World world, int x, int y, int z) : base(world, x, y, z) {

        //Set the x and y values to 16 times their inputed value, the z coordinate doesn't need to be changed
        setX(x * Chunk.chunkSize);
        setY(y * Chunk.chunkSize);

    }

    //Perform the same procedure, but this time a location variable is inputed
    public ChunkLocation(Location location) : base(location) {

        int x = location.getX();
        int y = location.getY();

        setX(x * Chunk.chunkSize);
        setY(y * Chunk.chunkSize);

    }

}