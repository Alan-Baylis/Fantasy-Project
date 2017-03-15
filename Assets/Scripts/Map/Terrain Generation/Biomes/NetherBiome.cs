
class Nether : Biome {

    public Nether() : base() {

        GeneratorData[] blockGenerationData = new GeneratorData[1];
        GeneratorData netherData = new GeneratorData(BlockType.NETHERRACK, 10, 0.04f, 5);

        blockGenerationData[0] = netherData;

        setGeneratorData(blockGenerationData);

        setCaveFrequency(0.5f);
        setCaveSize(15);

    }


}
