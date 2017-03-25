//The hill type of biome generates nice hills of stone capped with grass
class Hills : Biome {

    public Hills() : base() {

        //The biome generates for 3 different generation types
        GeneratorData[] blockGenerationData = new GeneratorData[3];
        //The stone hardpan/bedrock has a minimum height of 24 blocks, has valleys of ~20 blocks, and peaks of 4 blocks high
        GeneratorData stoneData = new GeneratorData(BlockType.STONE, 24, 0.05f, 4);
        //The stone hills start lower down than the hard pan, are 125 blocks apart and 48 blocks high
        GeneratorData mountainData = new GeneratorData(BlockType.STONE, -12, 0.008f, 48);
        //The grass caps atleast all stone with one block layer, and makes small 3 high lumps 20 blocks apart
        GeneratorData grassData = new GeneratorData(BlockType.GRASS, 1, 0.04f, 3);
        //Assign all the generator data in order
        blockGenerationData[0] = stoneData;
        blockGenerationData[1] = mountainData;
        blockGenerationData[2] = grassData;
        //set the generator data
        setGeneratorData(blockGenerationData);

    }


}
