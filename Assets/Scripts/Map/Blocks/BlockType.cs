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

public static class BlockTypeMethods {

    public static Block getBlock(this BlockType blockType, Location location) {

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
            default:
                return new Block(location);
        }
    }

}