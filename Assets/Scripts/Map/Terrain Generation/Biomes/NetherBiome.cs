//A subclass of the biome Class that generates terrain in a nether fashion
class Nether : Biome {

    public Nether() : base() {
        //This biome only generates terrain of one type for one block
        GeneratorData[] blockGenerationData = new GeneratorData[1];
        //Create generator Data for Netherrack, it has a minimum height of 10, a peak distance of 1/0.04 ~= 25 & each peak 
        //is ~= 5 blocks tall
        GeneratorData netherData = new GeneratorData(BlockType.NETHERRACK, 10, 0.04f, 5);
        //Set the generator data to the array
        blockGenerationData[0] = netherData;
        //set the generator data
        setGeneratorData(blockGenerationData);
        //set the cave size and frequency
        setCaveFrequency(0.5f);
        setCaveSize(15);

    }


}
