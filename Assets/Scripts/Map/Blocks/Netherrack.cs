using UnityEngine;

public class Netherrack : Block {

    public Netherrack(Location location) : base(location) {

        this.solid = true;
        this.renderable = true;
        this.destructable = true;

        this.blockType = BlockType.NETHERRACK;

        this.spriteName = "netherrack";

    }

}
