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

                chunkColumn = populateColumn(chunkColumn, getBiome());

                chunk.setColumn(columnBaseLocation, chunkColumn);

            }
        }

        return chunk;
    }

    public ChunkColumn populateColumn(ChunkColumn column, Biome biome) {

        Location columnLocation = column.getColumnBaseLocation();

        int x = columnLocation.getX();
        int y = columnLocation.getY();
        //Keep a tally of how high we are getting
        int currentHeight = 0;

        foreach(BlockType blockType in biome.getBlockTypes()) {

            GeneratorData generatorData = biome.getBlockGenerationData(blockType);

            int blockHeight = currentHeight + generatorData.getBlockBaseHeight();

            float blockNoise = generatorData.getBlockNoise();
            int blockBaseNoiseHeight = generatorData.getBlockBaseHeight();

            NoiseGenerator noiseGenerator = new NoiseGenerator(blockNoise, blockBaseNoiseHeight);

            int noise = noiseGenerator.generateNoise(x, y, 0);

            blockHeight += noise;

            for(int z = currentHeight; z < blockHeight; z++) {
                //Location blockLocation = new Location(x, y, z);
                //Block block = new Block(blockLocation, blockType);
                //column.setBlock(z, block);
                column.setBlockType(z, blockType);
            }
            //Debug.Log(blockType + " from " + currentHeight + " to " + blockHeight);
            currentHeight = blockHeight;
            
        }

        //int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        //stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

        //if(stoneHeight < stoneMinHeight)
        //    stoneHeight = Mathf.FloorToInt(stoneMinHeight);

        //stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        //int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        //dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        //for(int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++) {
        //    //Get a value to base cave generation on
        //    int caveChance = GetNoise(x, y, z, caveFrequency, 100);

        //    if(y <= stoneHeight && caveSize < caveChance) {
        //        SetBlock(x, y, z, new Block(), chunk);
        //    } else if(y <= dirtHeight && caveSize < caveChance) {
        //        SetBlock(x, y, z, new BlockGrass(), chunk);

        //        if(y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
        //            CreateTree(x, y + 1, z, chunk);
        //    } else {
        //        SetBlock(x, y, z, new BlockAir(), chunk);
        //    }

        //}

        return column;
    }
}
