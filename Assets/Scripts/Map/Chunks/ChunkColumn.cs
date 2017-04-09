using System.Collections.Generic;
using UnityEngine;
//The ChunkColumn object stores block data for a pillar of blocks at the given location up to a given world height, 
//it also is the world object that renders the highest block


//This makes sure that all chunk column GameObjects have a SpriteRenderer attached to them
[RequireComponent(typeof(SpriteRenderer))]
public class ChunkColumn : WorldObject {

    //The height of each column is fixed
    public static int blockMapHeight = 128;

    ////the block that is currently being rendered by the column
    //public Block block;

    //Get a reference to the sprite renderer
    private SpriteRenderer spriteRenderer;
    //The direction that the chunk column is facing
    private Direction facingDirection;
    //whether or not the column is being rendered or not
    private bool rendered = false;

    //Store a list of all block types in the column
    private List<Block> column = new List<Block>();
    //reference the location of the column, within it's chunk
    private Location columnBaseLocation;
    //The chunk the column is a member of
    private Chunk parentChunk;
    //The Biome of the column used for terrain generation
    private Biome biome;

    //Initialise is called when the chunk column is instantiated
    public void initialise(Chunk chunk, Biome biome, Location columnBaseLocation, Direction facingDirection) {

        //Set up the WorldObject at the given location
        base.initialise(columnBaseLocation);
        //assign the chunk holder
        this.parentChunk = chunk;
        //Assign the biome
        this.biome = biome;
        //and the direction the chunk is facing
        this.facingDirection = facingDirection;

        //Save the location of the chunk column
        this.columnBaseLocation = columnBaseLocation;

        float x = columnBaseLocation.getX();
        float z = columnBaseLocation.getZ();

        //Set up the empty lis of blocks in the column
        column = new List<Block>();

        //Initialise all blocks in the chunk column as air blocks
        for(int y = 0; y < blockMapHeight; y++) {
            Location location = new Location(getWorld(), x, y, z);
            column.Add(new Air(location));
        }
        //They start off invisible
        this.rendered = false;

        //Set the position of the column as the chunk's location and it's location within the chunk
        Location worldLocation = getWorldLocation();
        transform.position = worldLocation.getPosition();

        //recalculate it's rendering
        recalculateRendering();
    }

    //recalculateRendering() re sets the column's texture to the new heighest block in the column 
    public void recalculateRendering() {
        //Recalculate which block needs to be rendered
        Block block = getHighestRenderableBlock();

        //If the spriterenderer hasn't been referenced yet, get it
        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        // Get the sprite for this block
        Sprite blockSprite = block.getSprite(getDirection());
        //Set the sprite to the sprite renderer
        spriteRenderer.sprite = blockSprite;
        //move the column to the block's height in the column
        transform.position = block.getPosition();

    }

    //Assign the sprite renderer on load
    private void Start() {

        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.Rotate(Vector3.right * 90);

    }

    //Toggle render turns rendering if the column is rendered and turns it on if it isn't
    public void toggleRender() {
        //If the spriterenderer hasn't been referenced yet, get it
        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        //If the column is being rendered, then unrender the sprite mesh
        if(isRendered()) {
            spriteRenderer.enabled = false;
        } else {
            //Otherwise turn the sprite mesh back on
            spriteRenderer.enabled = true;
            //Get the block to be rendered
            Block block = getHighestRenderableBlock();
            //Get the sprite for this block
            Sprite blockSprite = block.getSprite(getDirection());
            //Set the sprite to the sprite renderer
            spriteRenderer.sprite = blockSprite;
            //move the column to the block's height in the column
            transform.position = block.getPosition();
        }
        //toggle the render tracking variable
        this.rendered = !isRendered();

    }

    //Returns the column's location on a global scale, as it's chunk location + it's location within the chunk
    public Location getWorldLocation() {
        return parentChunk.getLocation() + columnBaseLocation;
    }

    //returns all the blockTypes in the column as an array
    public Block[] getColumn() {
        return this.column.ToArray();
    }

    //Return the chunks biome
    public Biome getBiome() {
        return this.biome;
    }

    //Return the parent chunk
    public Chunk getChunk() {
        return this.parentChunk;
    }

    //Whether or not the block is being rendered
    public bool isRendered() {
        return this.rendered;
    }

    //The direction the chunk is facing (used to get the correct block texture)
    public Direction getDirection() {
        return this.facingDirection;
    }

    //Assign the block at the given z coord in the column to the given blockType
    public void setBlock(int y, Block block) {
        //remove the previous value and add the new one
        column.RemoveAt(y);
        column.Insert(y, block);
    }

    //Get the block type at the given z coordinate
    public Block getBlock(int y) {
        if(y < 0 || y > blockMapHeight) {
            return new Block(getLocation());
        }
        return column[y];
    }

    //Get the highest block in the column that has a texture
    public Block getHighestRenderableBlock() {

        //Iterate from the highest block in the column down to the lowest
        for(int y = blockMapHeight - 1; y >= 0; y--) {
            //Get the block type at this position in the column
            Block block = column[y];

            //if the block has a texture, we have found the heighest renderable block
            if(block.isRenderable()) {
                return block;
            }

        }
        //If there were no renderable blocks in the column, we default to a basic Block type at the lowest point in the column
        return new Block(getLocation());
    }

}

