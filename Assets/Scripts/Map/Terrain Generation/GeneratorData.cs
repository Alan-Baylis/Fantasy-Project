//This class is a data storage object about the generation data that is required to generate certain terrain for a block
public class GeneratorData {

    //The starting value for the generation block height
    private int blockBaseHeight;
    //the reciprocal of the spacing between peaks
    private float blockNoise;
    //the maximum separation between peaks and troughs
    private int blockNoiseHeight;

    //The block this data generates for
    private BlockType blockType;

    //Assign all given parameters in the constructor
    public GeneratorData(BlockType blockType, int blockBaseHeight, float blockNoise, int blockNoiseHeight) {

        this.blockType = blockType;
        this.blockBaseHeight = blockBaseHeight;
        this.blockNoise = blockNoise;
        this.blockNoiseHeight = blockNoiseHeight;

    }

    //Getters that return the relevant values

    public BlockType getBlockType() {
        return this.blockType;
    }

    public float getBlockNoise() {
        return this.blockNoise;
    }

    public int getBlockNoiseHeight() {
        return this.blockNoiseHeight;
    }

    public int getBlockBaseHeight() {
        return this.blockBaseHeight;
    }

}
