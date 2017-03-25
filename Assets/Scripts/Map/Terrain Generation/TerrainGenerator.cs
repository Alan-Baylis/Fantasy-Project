using UnityEngine;

//This class is the machinery behind all terrain generation that uses the data stored in the biome and GeneratorData
//to create terrain in the world
//it generates terrain on a column by column basis 
public class TerrainGenerator {

    //The biome stores information about waht blocks need to be generated
    private Biome biome;
    //The world for which the terrain will be generated
    private World world;

    //Constructor...
    public TerrainGenerator(World world, Biome biome) {

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
        chunk.initialise(chunkLocation, biome);

        //Get the x and y coordinates of the chunk
        int chunkX = chunk.getLocation().getX();
        int chunkY = chunk.getLocation().getY();

        //Iterate over all columns in the chunk
        for(int x = 0; x < Chunk.chunkSize; x++) {
            for(int y = 0; y < Chunk.chunkSize; y++) {

                //Get the coordinate of the column as the chunk's location in the world and the column's position in the
                //chunk, it's z-coord (height) can be 0
                Location columnBaseLocation = new Location(world, chunkX + x, chunkY + y, 0);

                //Instantieate a ChunkColumn GameObject from file aswell
                GameObject columnGameObject = MonoBehaviour.Instantiate(Resources.Load("Chunk Column", typeof(GameObject))) as GameObject;
                //Set the chunk column to be a child of the chunk in the heirarchy
                columnGameObject.transform.parent = chunkGameObject.transform;
                //Get the ChunkColumn script attached to this GameObject
                ChunkColumn chunkColumn = columnGameObject.GetComponent("ChunkColumn") as ChunkColumn;
                //initialise the variables in the chunkcolumn
                chunkColumn.initialise(chunk, columnBaseLocation, direction);

                //Fill the column with blocks based off of the biome
                chunkColumn = populateColumn(chunkColumn, biome);
                //Generate caves in the column (air spaces)
                chunkColumn = generateCaves(chunkColumn, biome);

                //Set the chunk column in the parent chunk to be this chunk column
                chunk.setColumn(columnBaseLocation, chunkColumn);

            }
        }
        //Return the newly generated chunk
        return chunk;
    }

    //This function fills the given chunk column with blocks based off of generation data found in the biome
    public ChunkColumn populateColumn(ChunkColumn column, Biome biome) {

        //Get the x and y coordinates from the chunk column's location
        Location columnLocation = column.getLocation();

        int x = columnLocation.getX();
        int y = columnLocation.getY();

        //Keep a tally of how high we are getting
        int currentHeight = 0;

        //Iterate over all the block types to be generated for in the biome
        foreach(BlockType blockType in biome.getBlockTypes()) {

            //Get all the genr=erationData for the given blocktype found in the biome
            GeneratorData[] blockGeneratorData = biome.getBlockGenerationData(blockType);
            //Iterate over all this generation data
            foreach(GeneratorData generatorData in blockGeneratorData) {
                //Initially add the minimum amount of the block that can occur to the current max height
                int blockHeight = currentHeight + generatorData.getBlockBaseHeight();
                //Get the noise value (reciprocal of valley width) for this block
                float blockNoise = generatorData.getBlockNoise();
                //Get the max height for the noise
                int blockBaseNoiseHeight = generatorData.getBlockNoiseHeight();
                //Generate block noise for this block type at the given coordinate using the values from generator Data
                int noise = NoiseGenerator.generateNoise(x, y, 0, blockNoise, blockBaseNoiseHeight);

                //Add the noise to the height of the terrain for this block
                blockHeight += noise;

                //Limit the height of the blocks reached to the map height limit
                if(blockHeight > ChunkColumn.blockMapHeight) {
                    blockHeight = ChunkColumn.blockMapHeight;
                }

                //Iterate over the new amount of blocks generated, and set the blocks at that position in the 
                //column to be this block
                for(int z = currentHeight; z < blockHeight; z++) {
                    column.setBlockType(z, blockType);
                }
                //Set the current total height to include the new blocks generated
                currentHeight = blockHeight;
            }
        }

        return column;
    }

    //Fills the column with air spaces to make caves
    public ChunkColumn generateCaves(ChunkColumn column, Biome biome) {

        //Get the coordinates af the column
        int x = column.getLocation().getX();
        int y = column.getLocation().getY();

        //Get the highest block's z coordinate
        int blockHeight = column.getHighestRenderableBlock().getZ();

        //Get the cave size and frequency from the biome
        int caveSize = biome.getCaveSize();
        float caveFrequency = biome.getCaveFrequency();

        //Create a noise generator that represents the probability of getting a cave as the caveFrequency as a percent
        NoiseGenerator noiseGenerator = new NoiseGenerator(caveFrequency, 100);

        //Iterate over all solid blocks in the column
        for(int z = 0; z < blockHeight + 1; z++) {

            //Get the probability of having a cave at this coordinate
            int caveChance = noiseGenerator.generateNoise(x, y, z);

            //If the cave chance is within the likely hood of generating a cave
            if(caveChance < caveSize) {
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
