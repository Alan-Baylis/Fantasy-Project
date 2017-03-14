using UnityEngine;

public class OakLog : Block {

    public OakLog(Location location) : base(location) {

        this.solid = true;
        this.renderable = true;
        this.destructable = true;

        this.blockType = BlockType.OAK_LOG;

        this.spriteName = "oak_log";

    }

}