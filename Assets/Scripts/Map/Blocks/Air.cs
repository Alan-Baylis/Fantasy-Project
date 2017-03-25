//Air is used as the filler block for all non assigned block locations
public class Air : Block {

    public Air(Location location) : base(location) {
        //You can walk through air and it is invisible so un renderable
        this.solid = false;
        this.renderable = false;
        //They don't break but are breakable... (poetic)
        this.destructable = true;

        this.blockType = BlockType.AIR;

    }

}
