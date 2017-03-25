public class Grass : Block {
    //The desctiptions are self explanatory
    public Grass(Location location) : base(location) {

        this.solid = true;
        this.renderable = true;
        this.destructable = true;

        this.blockType = BlockType.GRASS;

        this.spriteName = "grass";

    }

}
