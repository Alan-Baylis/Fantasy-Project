
class Hills : Biome {

    public Hills() : base() {

        GeneratorData[] blockGenerationData = new GeneratorData[3];
        GeneratorData stoneData = new GeneratorData(BlockType.STONE, 24, 0.05f, 4);
        GeneratorData mountainData = new GeneratorData(BlockType.STONE, -12, 0.008f, 48);
        GeneratorData grassData = new GeneratorData(BlockType.GRASS, 1, 0.04f, 3);
        blockGenerationData[0] = stoneData;
        blockGenerationData[1] = mountainData;
        blockGenerationData[2] = grassData;

        setGeneratorData(blockGenerationData);

    }


}
