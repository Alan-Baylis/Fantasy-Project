//The oak log block is a block type so it implements block 
public class OakLog : Block {

    //The constructor just passes all it's value to the Block constructor
    public OakLog(Location location) : base(location) {
        //Oak logs are solid
        this.solid = true;
        //They are renderable
        this.renderable = true;
        //They can be broken
        this.destructable = true;
        //Their blocktype is OAK_LOG
        this.blockType = BlockType.OAK_LOG;
        //Their texture name is oak_log
        this.spriteName = "oak_log";

    }

}