using System.Collections;
using UnityEngine;

//This is the core object that handles all game mechanics in a world
public class GameManager : MonoBehaviour {

    //Save a reference to the world the GameManager is handling
    private World world;
    //The prefab GameObject for all world objects
    public GameObject worldPrefab;

    //The event manager for the game
    private EventsManager eventsManager;

    //On start up, generate the world and create the events manager
    private void Start() {

        worldPrefab = Instantiate(worldPrefab, Vector3.zero, Quaternion.identity);
        this.world = worldPrefab.GetComponent<World>();
        this.world.initialise(new Location(null, 0, 0, 0), "WorldName");       
        this.world.gameManager = this;

        eventsManager = new EventsManager();

        //An example of how you would register an event handler to be a listener
        eventsManager.registerEventListener(new EventHandler());
        //An example of how you would call an event
        eventsManager.callEvent(new Event());

    }

    //Get the world object that has been generated
    public World getWorld() {
        return this.world;
    }

    //Return the reference to the events manager
    public EventsManager getEventsManager() {
        return this.eventsManager;
    }

    //This function fills the given chunk column with blocks based off of generation data found in the biome
    IEnumerator populateColumn(ChunkColumn column) {

        //Get the biome of the column
        Biome biome = column.getBiome();

        //Get the x and y coordinates from the chunk column's location
        Location columnLocation = column.getLocation();

        int x = columnLocation.getBlockX();
        int z = columnLocation.getBlockZ();

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
                int noise = NoiseGenerator.generateNoise(x, 0, z, blockNoise, blockBaseNoiseHeight);

                //Add the noise to the height of the terrain for this block
                blockHeight += noise;

                //Limit the height of the blocks reached to the map height limit
                if(blockHeight > ChunkColumn.blockMapHeight) {
                    blockHeight = ChunkColumn.blockMapHeight;
                }

                //Iterate over the new amount of blocks generated, and set the blocks at that position in the 
                //column to be this block
                for(int y = currentHeight; y < blockHeight; y++) {
                    Location blockLocation = new Location(getWorld(), x, y, z);
                    column.setBlock(y, blockType.getBlock(blockLocation));
                }
                //Set the current total height to include the new blocks generated
                currentHeight = blockHeight;

                yield return null;
            }
        }
        column.recalculateRendering();

    }

    //Fills the column with air spaces to make caves
    IEnumerator generateCaves(ChunkColumn column) {

        //Get the biome of the column
        Biome biome = column.getBiome();

        //Get the coordinates af the column
        int x = column.getLocation().getBlockX();
        int z = column.getLocation().getBlockZ();

        //Get the highest block's z coordinate
        int blockHeight = column.getHighestRenderableBlock().getBlockY();

        //Get the cave size and frequency from the biome
        int caveSize = biome.getCaveSize();
        float caveFrequency = biome.getCaveFrequency();

        //Create a noise generator that represents the probability of getting a cave as the caveFrequency as a percent
        NoiseGenerator noiseGenerator = new NoiseGenerator(caveFrequency, 100);

        //Iterate over all solid blocks in the column
        for(int y = 0; y < blockHeight + 1; y++) {

            //Get the probability of having a cave at this coordinate
            int caveChance = noiseGenerator.generateNoise(x, y, z);

            //If the cave chance is within the likely hood of generating a cave
            if(caveChance < caveSize) {
                Location airLocation = new Location(getWorld(), x, y, z);
                column.setBlock(y, new Air(airLocation));
            }
            yield return null;

        }
        column.recalculateRendering();
    }

    //void CreateTree(int x, int y, int z, Chunk chunk) {
    //    //create leaves
    //    for(int xi = -2; xi <= 2; xi++) {
    //        for(int yi = 4; yi <= 8; yi++) {
    //            for(int zi = -2; zi <= 2; zi++) {
    //                SetBlock(x + xi, y + yi, z + zi, new OakLeaves(), chunk, true);
    //            }
    //        }
    //    }

    //    //create trunk
    //    for(int yt = 0; yt < 6; yt++) {
    //        SetBlock(x, y + yt, z, new OakLog(), chunk, true);
    //    }
    //}

}