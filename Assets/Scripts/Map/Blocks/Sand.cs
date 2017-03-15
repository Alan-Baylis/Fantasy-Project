using UnityEngine;

public class Sand : Block {

    public Sand(Location location) : base(location) {

        this.solid = true;
        this.renderable = true;
        this.destructable = true;

        this.blockType = BlockType.SAND;

        this.spriteName = "sand";

    }

}
