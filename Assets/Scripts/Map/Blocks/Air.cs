public class Air : Block {

    public Air(Location location) : base(location) {

        this.solid = false;
        this.renderable = false;
        this.destructable = true;

        this.blockType = BlockType.AIR;

    }

}
