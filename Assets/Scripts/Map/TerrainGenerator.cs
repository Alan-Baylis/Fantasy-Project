using UnityEngine;
using System.Collections;

public class TerrainGenerator {

    private Biome biome;

    public TerrainGenerator(Biome biome) {

        this.biome = biome;

    }
  
    public Biome getBiome() {
        return this.biome;
    }

    public Chunk generateChunk(Location location) {

        Chunk chunk = new Chunk(location, biome);

        for(int x = 0; x < Chunk.chunkSize; x++) {
            for(int y = 0; y < Chunk.chunkSize; y++) {

                Location columnBaseLocation = new Location(x, y, 0);
                ChunkColumn chunkColumn = new ChunkColumn(columnBaseLocation);

                chunk.setColumn(columnBaseLocation, chunkColumn);

            }
        }

        return chunk;
    }
}
