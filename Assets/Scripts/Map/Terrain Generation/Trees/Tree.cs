//A data type that represents the generation of a tree in the world

//NOT implemented yet...
public class TerrainTree {
    //The type of the tree defines it's wood and leaf type
    private TreeType treeType;
    //The blocktypes used for wood and leaf blocks
    private BlockType leafType;
    private BlockType logType;
    //The width of the leaf cover around the tree
    private int foliageWidth;
    //The height above the starting point that the leaves will reach
    private int foliageHeight;
    //The height to which the tree trunk will grow
    private int trunkHeight;

    //The constructor assigns all the inputed values
    public TerrainTree(TreeType treeType, BlockType logType, int trunkHeight, BlockType leafType, int foliageWidth, int foliageHeight) {

        this.treeType = treeType;

        this.logType = logType;
        this.trunkHeight = trunkHeight;

        this.leafType = leafType;
        this.foliageWidth = foliageWidth;
        this.foliageHeight = foliageHeight;

    }
    //All the getters for the data in the object, pretty self explanatory

    public virtual TreeType getTreeType() {
        return this.treeType;
    }

    public virtual BlockType getLogType() {
        return this.logType;
    }

    public virtual BlockType getLeafType() {
        return this.leafType;
    }

    public virtual int getTrunkHeight() {
        return this.trunkHeight;
    }

    public virtual int getFoliageWidth() {
        return this.foliageWidth;
    }

    public virtual int getFoliageHeight() {
        return this.foliageHeight;
    }

}
