//Oak Leaved inherit from Block
public class OakLeaves : Block {
    //Pass the location to the base constructor
    public OakLeaves(Location location) : base(location) {
        //oak leaves are solid meaning you can't walk through them
        this.solid = true;
        //They can be rendered and broken
        this.renderable = true;
        this.destructable = true;
        //They are referenced by the enum OAK_LEAVES and theire texture file is of the same name
        this.blockType = BlockType.OAK_LEAVES;

        this.spriteName = "oak_leaves";

    }

}
