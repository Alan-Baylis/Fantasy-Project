using UnityEngine;
using System.Collections;
using System;

[Serializable]
public enum BlockType {

    BLOCK,
    AIR,
    STONE,
    DIRT,
    GRASS

}

[Serializable]
public struct BlockTexture {

    public BlockType blockType;
    public Sprite blockSprite;

}
