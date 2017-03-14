using UnityEngine;

public class TerrainGenerator {

    private Biome biome;
    private World world;

    public TerrainGenerator(World world, Biome biome) {

        this.biome = biome;
        this.world = world;

    }

    public Biome getBiome() {
        return this.biome;
    }

    public World getWorld() {
        return this.world;
    }

    public Chunk generateChunk(ChunkLocation chunkLocation, Direction direction) {

        Biome biome = getBiome();
        World world = chunkLocation.getWorld();

        GameObject chunkGameObject = MonoBehaviour.Instantiate(Resources.Load("Chunk", typeof(GameObject))) as GameObject;
        Chunk chunk = chunkGameObject.GetComponent("Chunk") as Chunk;
        chunk.initialise(chunkLocation, biome);

        int chunkX = chunk.getLocation().getX();
        int chunkY = chunk.getLocation().getY();

        for(int x = 0; x < Chunk.chunkSize; x++) {
            for(int y = 0; y < Chunk.chunkSize; y++) {

                Location columnBaseLocation = new Location(world, chunkX + x, chunkY + y, 0);

                GameObject columnGameObject = MonoBehaviour.Instantiate(Resources.Load("Chunk Column", typeof(GameObject))) as GameObject;
                columnGameObject.transform.parent = chunkGameObject.transform;

                ChunkColumn chunkColumn = columnGameObject.GetComponent("ChunkColumn") as ChunkColumn;
                chunkColumn.initialise(chunk, columnBaseLocation, direction);

                chunkColumn = populateColumn(chunkColumn, biome);
                chunkColumn = generateCaves(chunkColumn, biome);

                chunk.setColumn(columnBaseLocation, chunkColumn);

            }
        }

        return chunk;
    }

    public ChunkColumn populateColumn(ChunkColumn column, Biome biome) {

        Location columnLocation = column.getLocation();

        int x = columnLocation.getX();
        int y = columnLocation.getY();

        //Keep a tally of how high we are getting
        int currentHeight = 0;

        foreach(BlockType blockType in biome.getBlockTypes()) {

            GeneratorData[] blockGeneratorData = biome.getBlockGenerationData(blockType);

            foreach(GeneratorData generatorData in blockGeneratorData) {

                int blockHeight = currentHeight + generatorData.getBlockBaseHeight();

                float blockNoise = generatorData.getBlockNoise();
                int blockBaseNoiseHeight = generatorData.getBlockNoiseHeight();

                NoiseGenerator noiseGenerator = new NoiseGenerator(blockNoise, blockBaseNoiseHeight);

                int noise = noiseGenerator.generateNoise(x, y, 0);

                blockHeight += noise;

                if(blockHeight > ChunkColumn.blockMapHeight) {
                    blockHeight = ChunkColumn.blockMapHeight;
                }

                for(int z = currentHeight; z < blockHeight; z++) {
                    column.setBlockType(z, blockType);
                }
                currentHeight = blockHeight;
            }
        }

        return column;
    }

    public ChunkColumn generateCaves(ChunkColumn column, Biome biome) {

        int x = column.getLocation().getX();
        int y = column.getLocation().getY();

        int blockHeight = column.getHighestRenderableBlock().getZ();

        int caveSize = biome.getCaveSize();
        float caveFrequency = biome.getCaveFrequency();

        NoiseGenerator noiseGenerator = new NoiseGenerator(caveFrequency, 100);

        for(int z = 0; z < blockHeight; z++) {

            int caveChance = noiseGenerator.generateNoise(x, y, z);

            if(caveSize > caveChance) {
                column.setBlockType(z, BlockType.AIR);
            }

        }

        return column;

    }

    //void CreateTree(int x, int y, int z, Chunk chunk) {
        //        //create leaves
        //        for(int xi = -2; xi <= 2; xi++) {
        //            for(int yi = 4; yi <= 8; yi++) {
        //                for(int zi = -2; zi <= 2; zi++) {
        //                    SetBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
        //                }
        //            }
        //        }

        //        //create trunk
        //        for(int yt = 0; yt < 6; yt++) {
        //            SetBlock(x, y + yt, z, new BlockWood(), chunk, true);
        //        }
        //    }
    }
