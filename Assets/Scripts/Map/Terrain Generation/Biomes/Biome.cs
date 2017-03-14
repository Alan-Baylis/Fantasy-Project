using System.Collections.Generic;

public class Biome {

    //private Dictionary<BlockType, GeneratorData> blockGeneratorData = new Dictionary<BlockType, GeneratorData>();
    private List<GeneratorData> blockGeneratorData = new List<GeneratorData>();
    private List<BlockType> blockTypes = new List<BlockType>();

    private int caveSize;
    private float caveFrequency;

    public Biome() {

        this.caveSize = 7;
        this.caveFrequency = 0.025f;

    }

    public virtual void setGeneratorData(GeneratorData[] blockGenerationData) {

        //BlockType[] blockTypes = new BlockType[blockGenerationData.Length];

        foreach(GeneratorData generatorData in blockGenerationData) {

            //GeneratorData generatorData = blockGenerationData[i];

            BlockType blockType = generatorData.getBlockType();
            //blockGeneratorData.Add(blockType, generatorData);
            blockGeneratorData.Add(generatorData);
            blockTypes.Add(blockType);

            //blockTypes[i] = blockType;

        }

        //this.blockTypes = blockTypes;
    }

    public virtual List<BlockType> getBlockTypes() {
        return this.blockTypes;
    }

    public virtual GeneratorData[] getBlockGenerationData(BlockType blockType) {

        //if(blockGeneratorData.ContainsKey(blockType)) {
        //    return blockGeneratorData[blockType];
        //} else {
        //    return null;
        //}
        
        if(!blockTypes.Contains(blockType)) {
            return null;
        }

        List<GeneratorData> generatorData = new List<GeneratorData>();

        for(int i = 0; i < blockTypes.Count; i++) {

            BlockType type = blockTypes[i];
            if(type == blockType) {
                GeneratorData blockData = blockGeneratorData[i];
                generatorData.Add(blockData);
            }

        }
        return generatorData.ToArray();

    }

    public virtual int getCaveSize() {
        return this.caveSize;
    }

    public virtual void setCaveSize(int size) {
        this.caveSize = size;
    }

    public virtual float getCaveFrequency() {
        return this.caveFrequency;
    }

    public void setCaveFrequency(float frequency) {
        this.caveFrequency = frequency;
    }
}
