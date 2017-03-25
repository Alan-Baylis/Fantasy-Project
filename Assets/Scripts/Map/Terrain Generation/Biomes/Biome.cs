using System.Collections.Generic;

//Biomes contain the data necessary for terrain generation of a certain style using certain blocks
public class Biome {

    //A list of the generator data for each block type, the two lists are in sync
    private List<GeneratorData> blockGeneratorData = new List<GeneratorData>();
    private List<BlockType> blockTypes = new List<BlockType>();

    //The average size of a cave in the biome
    private int caveSize;
    //The likely hood of a cave being generated at a location
    private float caveFrequency;

    //Initialise the default values of all biomes
    public Biome() {

        //caves default to 7 blocks wide/tall
        this.caveSize = 7;
        //You are 2.5% likely to generate a cave at a given block
        this.caveFrequency = 0.025f;

    }

    //Set all generation data for blocks in the biome to the inputed array
    public virtual void setGeneratorData(GeneratorData[] blockGenerationData) {

        //Reset the current data for block generation
        blockGeneratorData = new List<GeneratorData>();
        blockTypes = new List<BlockType>();

        //Iterate over all the GeneratorData given
        foreach(GeneratorData generatorData in blockGenerationData) {
            //Get the Block for whuch this data generates features
            BlockType blockType = generatorData.getBlockType();
            //Add the data to the list of generator data
            blockGeneratorData.Add(generatorData);
            //Add the block to the list of blocks
            blockTypes.Add(blockType);

        }
    }

    //Get the list of blocktypes for which this biome generates data
    public virtual List<BlockType> getBlockTypes() {
        return this.blockTypes;
    }

    //Get all the generator data for a given block type
    public virtual GeneratorData[] getBlockGenerationData(BlockType blockType) {

        //If the block isn't used to generate features in the biome, return null
        if(!blockTypes.Contains(blockType)) {
            return null;
        }
        //Make an empty list of all the generator data that the block has
        List<GeneratorData> generatorData = new List<GeneratorData>();
        //Iterate over all blocks that the biome generates for
        for(int i = 0; i < blockTypes.Count; i++) {
            //Get the block type that is generated for
            BlockType type = blockTypes[i];
            //If this blockType is the blockType we are looking for, add it's generatorData to the list
            if(type == blockType) {
                GeneratorData blockData = blockGeneratorData[i];
                generatorData.Add(blockData);
            }

        }
        //return the list of generator data as an array
        return generatorData.ToArray();

    }

    //return the average size of a cave
    public virtual int getCaveSize() {
        return this.caveSize;
    }

    //set the average size of a cave
    public virtual void setCaveSize(int size) {
        this.caveSize = size;
    }

    //get the likelyhood of making a cave at any block in the biome
    public virtual float getCaveFrequency() {
        return this.caveFrequency;
    }

    //set the cave frequency
    public void setCaveFrequency(float frequency) {
        this.caveFrequency = frequency;
    }
}
