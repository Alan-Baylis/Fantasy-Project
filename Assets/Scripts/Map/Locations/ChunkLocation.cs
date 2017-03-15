using UnityEngine;

public class ChunkLocation : Location {

    public ChunkLocation(World world, int x, int y, int z) : base(world, x, y, z) {

        setX(x * Chunk.chunkSize);
        setY(y * Chunk.chunkSize);
        setZ(z * Chunk.chunkSize);

    }

    public ChunkLocation(World world, Vector3 position) : base(world, position) {

    }

    public ChunkLocation(Location location) : base(location) {

        int x = location.getX();
        int y = location.getY();
        int z = location.getZ();

        setX(x * Chunk.chunkSize);
        setY(y * Chunk.chunkSize);
        setZ(z * Chunk.chunkSize);

    }

}