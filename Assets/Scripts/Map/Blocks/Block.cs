using System;
using UnityEngine;

//The base class to describe all blocks in the game
[Serializable]
public class Block : Location {

    //public Location location;

    public bool destructable = true;
    public bool renderable = false;
    public bool solid = false;

    public BlockType blockType = BlockType.BLOCK;
    public Sprite blockSprite = new Sprite();

    public string spriteName;

	public Block(Location location) : base(location) {

        //this.location = location;

    }

    //public World getWorld() {
    //    return getLocation().getWorld();
    //}

    //public Location getLocation() {
    //    return this.location;
    //}

    //Solid decides whether or not the player can walk through the block
    public virtual bool isSolid() {
        return solid;
    }

    //Whether or not the block can be destroyed in the game,
    //by default all blocks can be destroyed
    public virtual bool isDestructable() {
        return destructable;
    }

    public virtual bool isRenderable() {
        return renderable;
    }

    public virtual Sprite getSprite(Direction direction) {

        //spriteName += "_" + direction.ToString(); 

        Sprite sprite = Resources.Load<Sprite>(spriteName);
        return sprite;

    }

    public virtual BlockType getBlockType() {
        return blockType;
    }

}
