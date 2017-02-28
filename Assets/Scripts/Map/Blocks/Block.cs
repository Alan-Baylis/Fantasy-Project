using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The base class to describe all blocks in the game
[Serializable]
public class Block {

    //The size of the block, relative to unity coordinates
    public static float blockSize = 1f;

    //It's location in the world
    public Location location;
    //The texture of the block
    public SpriteRenderer sprite;

	public Block(Location location) {

        this.location = location;
        //sprite = GetComponent<SpriteRenderer>();

    }

    //private void Start() {

    //sprite = GetComponent<SpriteRenderer>();
    //Debug.Log(sprite.sprite);

    //}

    //Get the location of the block
    public virtual Location getLocation() {
        return this.location;
    }

    //Solid decides whether or not the player can walk through the block
    public virtual bool isSolid() {
        return false;
    }

    //Whether or not the block can be destroyed in the game,
    //by default all blocks can be destroyed
    public virtual bool isDestructable() {
        return true;
    }


}
