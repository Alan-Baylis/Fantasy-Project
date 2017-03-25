using System.Collections.Generic;
using UnityEngine;
//The ChunkColumn object stores block data for a pillar of blocks at the given location up to a given world height, 
//it also is the world object that renders the highest block


//This makes sure that all chunk column GameObjects have a SpriteRenderer attached to them
[RequireComponent(typeof(SpriteRenderer))]
public class ChunkColumn : WorldObject {

    //The height of each column is fixed
    public static int blockMapHeight = 128;

    //the block that is currently being rendered by the column
    public Block block;

    //Get a reference to the sprite renderer
    private SpriteRenderer spriteRenderer;
    //The direction that the chunk column is facing
    private Direction facingDirection;
    //whether or not the column is being rendered or not
    private bool rendered = false;

    //Store a list of all block types in the column
    private List<BlockType> column = new List<BlockType>();
    //reference the location of the column, within it's chunk
    private Location columnBaseLocation;
    //The chunk the column is a member of
    private Chunk parentChunk;

    //Initialise is called when the chunk column is instantiated
    public void initialise(Chunk chunk, Location columnBaseLocation, Direction facingDirection) {

        //Set up the WorldObject at the given location
        base.initialise(columnBaseLocation);
        //assign the chunk holder
        this.parentChunk = chunk;
        //and the direction the chunk is facing
        this.facingDirection = facingDirection;

        //Save the location of the chunk column
        this.columnBaseLocation = columnBaseLocation;
        //Initialise all blocks in the chunk column as air blocks
        for(int z = 0; z < blockMapHeight; z++) {
            column.Add(BlockType.AIR);
        }
        //They start off invisible
        this.rendered = false;
        //The block is the highest block that has a texture in the columns, if they are all air blocks, this will be 
        //a basic Block() type
        block = getHighestRenderableBlock();

        //Set the position of the column as the chunk's location and it's location within the chunk
        transform.position = parentChunk.getLocation().asTransform() + columnBaseLocation.asTransform();
    }

    //update is not the same update as is called be Unity
    public void update() {
        //Recalculate which block needs to be rendered
        block = getHighestRenderableBlock();

    }
    //Assign the sprite renderer on load
    private void Start() {

        spriteRenderer = GetComponent<SpriteRenderer>();

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
            transform.position = block.asTransform();
        }
        //togglt the render tracking variable
        this.rendered = !isRendered();

    }

    //Returns the column's location on a global scale, as it's chunk location + it's location within the chunk
    public Location getWorldLocation() {
        return parentChunk.getLocation() + columnBaseLocation;
    }

    //returns all the blockTypes in the column as an array
    public BlockType[] getColumn() {
        return this.column.ToArray();
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
    public void setBlockType(int z, BlockType blockType) {
        //remove the previous value and add the new one
        column.RemoveAt(z);
        column.Insert(z, blockType);
    }

    //Get the block type at the given z coordinate
    public BlockType getBlockType(int z) {
        if(z < 0 || z > blockMapHeight) {
            return BlockType.BLOCK;
        }
        return column[z];
    }

    //Get the highest block in the column that has a texture
    public Block getHighestRenderableBlock() {

        //Iterate from the highest block in the column down to the lowest
        for(int z = blockMapHeight - 1; z >= 0; z--) {
            //Get the block type at this position in the column
            BlockType blockType = column[z];
            //get the location of the column in the chunk and set the z coord to the current height
            Location location = getLocation();
            location.setZ(z);
            //Block block = Block.getBlock(world, blockType, location);
            //Get an instance of the given block types's associated block at this cooredinate
            Block block = blockType.getBlock(location);

            //if the block has a texture, we have found the heighest renderable block
            if(block.isRenderable()) {
                return block;
            }

        }
        //If there were no renderable blocks in the column, we default to a basic Block type at the lowest point in the column
        return new Block(getLocation());
    }

}

