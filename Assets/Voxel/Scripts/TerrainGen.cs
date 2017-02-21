using UnityEngine;
using System.Collections;
using SimplexNoise;

//The class that handles the block generation within each chunk
public class TerrainGenerator {
    //The height at which stone blocks start generating
    float stoneBaseHeight = -24;
    //Noise modifier for generating stone bases
    float stoneBaseNoise = 0.05f;
    //The corresponding height for the noise
    float stoneBaseNoiseHeight = 4;

    //Same again, but for hills
    float stoneMountainHeight = 48;
    float stoneMountainFrequency = 0.008f;
    float stoneMinHeight = -12;

    //...you know, but it's dirt this time
    float dirtBaseHeight = 1;
    float dirtNoise = 0.04f;
    float dirtNoiseHeight = 3;

    //take a wild guess will ya?
    float caveFrequency = 0.025f;
    //the guessing contniues...
    int caveSize = 7;

    //mhmm
    float treeFrequency = 0.2f;
    int treeDensity = 3;

    //the function that generates the blocks procedurally for a given chunk
    public Chunk ChunkGen(Chunk chunk) {
        //Iterate over the coordinates in the chunk along the xz plane, with a 3 block border
        for(int x = chunk.pos.x - 3; x < chunk.pos.x + Chunk.chunkSize + 3; x++) {
            for(int z = chunk.pos.z - 3; z < chunk.pos.z + Chunk.chunkSize + 3; z++) {
                //Generate a new collumn for this coordinate, hence the no y axis iteration
                chunk = chunkColumnGen(chunk, x, z);
            }
        }
        //raturn the newly generated chunk
        return chunk;
    }

    //This function generates a new column of blocks (1*1) using noise,
    //it takes in the chunk to populate and he xz coordinates for the column
    public Chunk chunkColumnGen(Chunk chunk, int x, int z) {
        
        //Start the height at which stone blocks reach as the base height for stone
        int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
        //Add to this value a randomish (within a range) amount of extra height using the noise generator
        stoneHeight += GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));
        //Make sure the height is atleast the minimum
        if(stoneHeight < stoneMinHeight)
            stoneHeight = Mathf.FloorToInt(stoneMinHeight);
        //repeat the noise addition for the height of the base of the stone
        stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

        //
        int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
        dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

        for(int y = chunk.pos.y - 8; y < chunk.pos.y + Chunk.chunkSize; y++) {
            //Get a value to base cave generation on
            int caveChance = GetNoise(x, y, z, caveFrequency, 100);

            if(y <= stoneHeight && caveSize < caveChance) {
                setBlock(x, y, z, new Block(), chunk);
            } else if(y <= dirtHeight && caveSize < caveChance) {
                setBlock(x, y, z, new BlockGrass(), chunk);

                if(y == dirtHeight && GetNoise(x, 0, z, treeFrequency, 100) < treeDensity)
                    CreateTree(x, y + 1, z, chunk);
            } else {
                setBlock(x, y, z, new BlockAir(), chunk);
            }

        }

        return chunk;
    }

    void CreateTree(int x, int y, int z, Chunk chunk) {
        //create leaves
        for(int xi = -2; xi <= 2; xi++) {
            for(int yi = 4; yi <= 8; yi++) {
                for(int zi = -2; zi <= 2; zi++) {
                    setBlock(x + xi, y + yi, z + zi, new BlockLeaves(), chunk, true);
                }
            }
        }

        //create trunk
        for(int yt = 0; yt < 6; yt++) {
            setBlock(x, y + yt, z, new BlockWood(), chunk, true);
        }
    }

    public static void setBlock(int x, int y, int z, Block block, Chunk chunk, bool replaceBlocks = false) {
        x -= chunk.pos.x;
        y -= chunk.pos.y;
        z -= chunk.pos.z;

        if(Chunk.inRange(x) && Chunk.inRange(y) && Chunk.inRange(z)) {
            if(replaceBlocks || chunk.blocks[x, y, z] == null)
                chunk.setBlock(x, y, z, block);
        }
    }

    public static int GetNoise(int x, int y, int z, float scale, int max) {
        return Mathf.FloorToInt((Noise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
    }
}