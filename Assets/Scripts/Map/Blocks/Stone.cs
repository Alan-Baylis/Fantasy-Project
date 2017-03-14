using UnityEngine;

public class Stone : Block {

    public Stone(Location location) : base(location) {

        this.solid = true;
        this.renderable = true;
        this.destructable = true;

        this.blockType = BlockType.STONE;

        this.spriteName = "stone";
    }

}
