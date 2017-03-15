using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ChunkColumn : WorldObject {

    public static int blockMapHeight = 128;

    public Block block;

    private SpriteRenderer spriteRenderer;
    private Direction facingDirection;
    private bool rendered = false;

    private List<BlockType> column = new List<BlockType>();
    private Location columnBaseLocation;
    private Chunk parentChunk;

    public void initialise(Chunk chunk, Location columnBaseLocation, Direction facingDirection) {

        base.initialise(columnBaseLocation);

        this.parentChunk = chunk;
        this.facingDirection = facingDirection;

        this.columnBaseLocation = columnBaseLocation;
        for(int z = 0; z < blockMapHeight; z++) {
            column.Add(BlockType.AIR);
        }

        this.rendered = false;

        block = getHighestRenderableBlock();

        transform.position = parentChunk.getLocation().asTransform() + columnBaseLocation.asTransform();
    }

    public void update() {

        block = getHighestRenderableBlock();

        transform.position = parentChunk.getLocation().asTransform() + columnBaseLocation.asTransform();

    }

    private void Start() {

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void toggleRender() {

        if(spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if(isRendered()) {
            spriteRenderer.enabled = false;
        } else {
            spriteRenderer.enabled = true;
            Block block = getHighestRenderableBlock();
            Sprite blockSprite = block.getSprite(getDirection());
            spriteRenderer.sprite = blockSprite;
            transform.position = block.asTransform();
        }

        this.rendered = !isRendered();

    }

    //public Location getColumnBaseLocation() {
    //    return this.columnBaseLocation;
    //}

    public BlockType[] getColumn() {
        return this.column.ToArray();
    }

    public bool isRendered() {
        return this.rendered;
    }

    public Direction getDirection() {
        return this.facingDirection;
    }

    public void setBlockType(int z, BlockType blockType) {
        column.RemoveAt(z);
        column.Insert(z, blockType);
    }

    public Block getHighestRenderableBlock() {

        for(int z = blockMapHeight - 1; z >= 0; z--) {

            BlockType blockType = column[z];
            Location location = getLocation();
            location.setZ(z);
            //Block block = Block.getBlock(world, blockType, location);
            Block block = blockType.getBlock(location);

            if(block.isRenderable()) {
                return block;
            }

        }
        return new Block(getLocation());
    }

}

