using UnityEngine;

public class OakLeaves : Block {

    public OakLeaves(Location location) : base(location) {

        this.solid = true;
        this.renderable = true;
        this.destructable = true;

        this.blockType = BlockType.OAK_LEAVES;

        this.spriteName = "oak_leaves";

    }

}
