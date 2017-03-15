
public class Tree {

    private TreeType treeType;

    private BlockType leafType;
    private BlockType logType;

    private int foliageWidth;
    private int foliageHeight;

    private int trunkHeight;

    public Tree(TreeType treeType, BlockType logType, int trunkHeight, BlockType leafType, int foliageWidth, int foliageHeight) {

        this.treeType = treeType;

        this.logType = logType;
        this.trunkHeight = trunkHeight;

        this.leafType = leafType;
        this.foliageWidth = foliageWidth;
        this.foliageHeight = foliageHeight;

    }

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
