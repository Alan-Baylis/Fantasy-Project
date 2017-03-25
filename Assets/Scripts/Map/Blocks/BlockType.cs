//This enum represents all blocks in the game without having to make an instance of that block at a given location
public enum BlockType {

    BLOCK,

    AIR,

    STONE,
    GRASS,
    SAND,

    NETHERRACK,

    OAK_LOG,
    OAK_LEAVES

    

}
//This class adds methods that can be used with the BlockType enum
public static class BlockTypeMethods {
    //This method creates an instance of a block of it's block type at the given location
    public static Block getBlock(this BlockType blockType, Location location) {
        //Switch over the blocktypes and create an instance for the correct block type
        switch(blockType) {
            case BlockType.AIR:
                return new Air(location);
            case BlockType.GRASS:
                return new Grass(location);
            case BlockType.SAND:
                return new Sand(location);
            case BlockType.STONE:
                return new Stone(location);
            case BlockType.NETHERRACK:
                return new Netherrack(location);
            case BlockType.OAK_LOG:
                return new OakLog(location);
            case BlockType.OAK_LEAVES:
                return new OakLeaves(location);
                //By default just create a Block base type
            default:
                return new Block(location);
        }
    }

}