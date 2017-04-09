using System;
using UnityEngine;

//The base class to describe all blocks in the game
[Serializable]
//All blocks are a subclass of location so they inherit it's methods
public class Block : Location {

    //Descibes whether or not you can break the block, defaults to true
    public bool destructable = true;
    //Blocks by default aren't rendered, so they don't all need a texture
    public bool renderable = false;
    //By default you can walk through all blocks
    public bool solid = false;

    //Their default type is just block, this is used to create an instance of a deisred block at a given location
    public BlockType blockType = BlockType.BLOCK;
    //They have a textureless sprite by default
    public Sprite blockSprite = new Sprite();
    //The file name/path of the texure for the block
    public string spriteName;

    //Since Block inherits from location, you can pass it's location to the super class to be handled
	public Block(Location location) : base(location) {

    }

    //Solid decides whether or not the player can walk through the block
    public virtual bool isSolid() {
        return solid;
    }

    //Whether or not the block can be destroyed in the game,
    //by default all blocks can be destroyed
    public virtual bool isDestructable() {
        return destructable;
    }

    //Whether or not the block needs to have a texture and be visible in the game, by default they are invisible
    public virtual bool isRenderable() {
        return renderable;
    }

    //Get the corresponding sprite for the direction that the player is facing
    public virtual Sprite getSprite(Direction direction) {

        //spriteName += "_" + direction.ToString(); 

        //Load the texture as a sprite from the specified file name
        Sprite sprite = Resources.Load<Sprite>(spriteName);
        return sprite;

    }
    //Return the type of block the block is
    public virtual BlockType getBlockType() {
        return blockType;
    }

    public Block getNeighbouringBlock(Direction direction) {

        World world = getWorld();
        Location neighbouringLocation = getNeighbouringLocation(direction);
        Chunk chunk = world.getChunk(neighbouringLocation);
        ChunkColumn chunkColumn = chunk.getColumn(neighbouringLocation);
        Block neighbouringBlock = chunkColumn.getBlock(neighbouringLocation.getBlockZ());

        return neighbouringBlock;
    }

}
