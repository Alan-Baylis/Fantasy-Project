//A subclass of the tree class, it describes tree shapes for oak tress
public class Oak : TerrainTree {

    //The trunk is 6 blocks high and leaves suround it in a 4*4 cube
    public Oak() : base(TreeType.OAK, BlockType.OAK_LOG, 6, BlockType.OAK_LEAVES, 4, 4) { 

    }

}
