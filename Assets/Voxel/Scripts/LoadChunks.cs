﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadChunks : MonoBehaviour
{
    static WorldPosition[] chunkPositions = {   new WorldPosition( 0, 0,  0), new WorldPosition(-1, 0,  0), new WorldPosition( 0, 0, -1), new WorldPosition( 0, 0,  1), new WorldPosition( 1, 0,  0),
                             new WorldPosition(-1, 0, -1), new WorldPosition(-1, 0,  1), new WorldPosition( 1, 0, -1), new WorldPosition( 1, 0,  1), new WorldPosition(-2, 0,  0),
                             new WorldPosition( 0, 0, -2), new WorldPosition( 0, 0,  2), new WorldPosition( 2, 0,  0), new WorldPosition(-2, 0, -1), new WorldPosition(-2, 0,  1),
                             new WorldPosition(-1, 0, -2), new WorldPosition(-1, 0,  2), new WorldPosition( 1, 0, -2), new WorldPosition( 1, 0,  2), new WorldPosition( 2, 0, -1),
                             new WorldPosition( 2, 0,  1), new WorldPosition(-2, 0, -2), new WorldPosition(-2, 0,  2), new WorldPosition( 2, 0, -2), new WorldPosition( 2, 0,  2),
                             new WorldPosition(-3, 0,  0), new WorldPosition( 0, 0, -3), new WorldPosition( 0, 0,  3), new WorldPosition( 3, 0,  0), new WorldPosition(-3, 0, -1),
                             new WorldPosition(-3, 0,  1), new WorldPosition(-1, 0, -3), new WorldPosition(-1, 0,  3), new WorldPosition( 1, 0, -3), new WorldPosition( 1, 0,  3),
                             new WorldPosition( 3, 0, -1), new WorldPosition( 3, 0,  1), new WorldPosition(-3, 0, -2), new WorldPosition(-3, 0,  2), new WorldPosition(-2, 0, -3),
                             new WorldPosition(-2, 0,  3), new WorldPosition( 2, 0, -3), new WorldPosition( 2, 0,  3), new WorldPosition( 3, 0, -2), new WorldPosition( 3, 0,  2),
                             new WorldPosition(-4, 0,  0), new WorldPosition( 0, 0, -4), new WorldPosition( 0, 0,  4), new WorldPosition( 4, 0,  0), new WorldPosition(-4, 0, -1),
                             new WorldPosition(-4, 0,  1), new WorldPosition(-1, 0, -4), new WorldPosition(-1, 0,  4), new WorldPosition( 1, 0, -4), new WorldPosition( 1, 0,  4),
                             new WorldPosition( 4, 0, -1), new WorldPosition( 4, 0,  1), new WorldPosition(-3, 0, -3), new WorldPosition(-3, 0,  3), new WorldPosition( 3, 0, -3),
                             new WorldPosition( 3, 0,  3), new WorldPosition(-4, 0, -2), new WorldPosition(-4, 0,  2), new WorldPosition(-2, 0, -4), new WorldPosition(-2, 0,  4),
                             new WorldPosition( 2, 0, -4), new WorldPosition( 2, 0,  4), new WorldPosition( 4, 0, -2), new WorldPosition( 4, 0,  2), new WorldPosition(-5, 0,  0),
                             new WorldPosition(-4, 0, -3), new WorldPosition(-4, 0,  3), new WorldPosition(-3, 0, -4), new WorldPosition(-3, 0,  4), new WorldPosition( 0, 0, -5),
                             new WorldPosition( 0, 0,  5), new WorldPosition( 3, 0, -4), new WorldPosition( 3, 0,  4), new WorldPosition( 4, 0, -3), new WorldPosition( 4, 0,  3),
                             new WorldPosition( 5, 0,  0), new WorldPosition(-5, 0, -1), new WorldPosition(-5, 0,  1), new WorldPosition(-1, 0, -5), new WorldPosition(-1, 0,  5),
                             new WorldPosition( 1, 0, -5), new WorldPosition( 1, 0,  5), new WorldPosition( 5, 0, -1), new WorldPosition( 5, 0,  1), new WorldPosition(-5, 0, -2),
                             new WorldPosition(-5, 0,  2), new WorldPosition(-2, 0, -5), new WorldPosition(-2, 0,  5), new WorldPosition( 2, 0, -5), new WorldPosition( 2, 0,  5),
                             new WorldPosition( 5, 0, -2), new WorldPosition( 5, 0,  2), new WorldPosition(-4, 0, -4), new WorldPosition(-4, 0,  4), new WorldPosition( 4, 0, -4),
                             new WorldPosition( 4, 0,  4), new WorldPosition(-5, 0, -3), new WorldPosition(-5, 0,  3), new WorldPosition(-3, 0, -5), new WorldPosition(-3, 0,  5),
                             new WorldPosition( 3, 0, -5), new WorldPosition( 3, 0,  5), new WorldPosition( 5, 0, -3), new WorldPosition( 5, 0,  3), new WorldPosition(-6, 0,  0),
                             new WorldPosition( 0, 0, -6), new WorldPosition( 0, 0,  6), new WorldPosition( 6, 0,  0), new WorldPosition(-6, 0, -1), new WorldPosition(-6, 0,  1),
                             new WorldPosition(-1, 0, -6), new WorldPosition(-1, 0,  6), new WorldPosition( 1, 0, -6), new WorldPosition( 1, 0,  6), new WorldPosition( 6, 0, -1),
                             new WorldPosition( 6, 0,  1), new WorldPosition(-6, 0, -2), new WorldPosition(-6, 0,  2), new WorldPosition(-2, 0, -6), new WorldPosition(-2, 0,  6),
                             new WorldPosition( 2, 0, -6), new WorldPosition( 2, 0,  6), new WorldPosition( 6, 0, -2), new WorldPosition( 6, 0,  2), new WorldPosition(-5, 0, -4),
                             new WorldPosition(-5, 0,  4), new WorldPosition(-4, 0, -5), new WorldPosition(-4, 0,  5), new WorldPosition( 4, 0, -5), new WorldPosition( 4, 0,  5),
                             new WorldPosition( 5, 0, -4), new WorldPosition( 5, 0,  4), new WorldPosition(-6, 0, -3), new WorldPosition(-6, 0,  3), new WorldPosition(-3, 0, -6),
                             new WorldPosition(-3, 0,  6), new WorldPosition( 3, 0, -6), new WorldPosition( 3, 0,  6), new WorldPosition( 6, 0, -3), new WorldPosition( 6, 0,  3),
                             new WorldPosition(-7, 0,  0), new WorldPosition( 0, 0, -7), new WorldPosition( 0, 0,  7), new WorldPosition( 7, 0,  0), new WorldPosition(-7, 0, -1),
                             new WorldPosition(-7, 0,  1), new WorldPosition(-5, 0, -5), new WorldPosition(-5, 0,  5), new WorldPosition(-1, 0, -7), new WorldPosition(-1, 0,  7),
                             new WorldPosition( 1, 0, -7), new WorldPosition( 1, 0,  7), new WorldPosition( 5, 0, -5), new WorldPosition( 5, 0,  5), new WorldPosition( 7, 0, -1),
                             new WorldPosition( 7, 0,  1), new WorldPosition(-6, 0, -4), new WorldPosition(-6, 0,  4), new WorldPosition(-4, 0, -6), new WorldPosition(-4, 0,  6),
                             new WorldPosition( 4, 0, -6), new WorldPosition( 4, 0,  6), new WorldPosition( 6, 0, -4), new WorldPosition( 6, 0,  4), new WorldPosition(-7, 0, -2),
                             new WorldPosition(-7, 0,  2), new WorldPosition(-2, 0, -7), new WorldPosition(-2, 0,  7), new WorldPosition( 2, 0, -7), new WorldPosition( 2, 0,  7),
                             new WorldPosition( 7, 0, -2), new WorldPosition( 7, 0,  2), new WorldPosition(-7, 0, -3), new WorldPosition(-7, 0,  3), new WorldPosition(-3, 0, -7),
                             new WorldPosition(-3, 0,  7), new WorldPosition( 3, 0, -7), new WorldPosition( 3, 0,  7), new WorldPosition( 7, 0, -3), new WorldPosition( 7, 0,  3),
                             new WorldPosition(-6, 0, -5), new WorldPosition(-6, 0,  5), new WorldPosition(-5, 0, -6), new WorldPosition(-5, 0,  6), new WorldPosition( 5, 0, -6),
                             new WorldPosition( 5, 0,  6), new WorldPosition( 6, 0, -5), new WorldPosition( 6, 0,  5) };

    public World world;

    List<WorldPosition> updateList = new List<WorldPosition>();
    List<WorldPosition> buildList = new List<WorldPosition>();

    int timer = 0;

    // Update is called once per frame
    void Update()
    {
        if (DeleteChunks())
            return;

        FindChunksToLoad();
        LoadAndRenderChunks();
    }

    void FindChunksToLoad()
    {
        //Get the position of this gameobject to generate around
        WorldPosition playerPos = new WorldPosition(
            Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize,
            Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize
            );

        //If there aren't already chunks to generate
        if (updateList.Count == 0)
        {
            //Cycle through the array of positions
            for (int i = 0; i < chunkPositions.Length; i++)
            {
                //translate the player position and array position into chunk position
                WorldPosition newChunkPos = new WorldPosition(
                    chunkPositions[i].x * Chunk.chunkSize + playerPos.x,
                    0,
                    chunkPositions[i].z * Chunk.chunkSize + playerPos.z
                    );

                //Get the chunk in the defined position
                Chunk newChunk = world.GetChunk(
                    newChunkPos.x, newChunkPos.y, newChunkPos.z);

                //If the chunk already exists and it's already
                //rendered or in queue to be rendered continue
                if (newChunk != null
                    && (newChunk.rendered || updateList.Contains(newChunkPos)))
                    continue;

                //load a column of chunks in this position
                for (int y = -4; y < 4; y++)
                {

                    for (int x = newChunkPos.x - Chunk.chunkSize; x <= newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                    {
                        for (int z = newChunkPos.z - Chunk.chunkSize; z <= newChunkPos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                        {
                            buildList.Add(new WorldPosition(
                                x, y * Chunk.chunkSize, z));
                        }
                    }
                    updateList.Add(new WorldPosition(
                                newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                }
                return;
            }
        }
    }

    void LoadAndRenderChunks()
    {
        if (buildList.Count != 0)
        {
            for (int i = 0; i < buildList.Count && i < 8; i++)
            {
                BuildChunk(buildList[0]);
                buildList.RemoveAt(0);
            }

            //If chunks were built return early
            return;
        }

        if (updateList.Count != 0)
        {
            Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
            if (chunk != null)
                chunk.update = true;
            updateList.RemoveAt(0);
        }
    }

    void BuildChunk(WorldPosition pos)
    {
        if (world.GetChunk(pos.x, pos.y, pos.z) == null)
            world.CreateChunk(pos.x, pos.y, pos.z);
    }

    bool DeleteChunks()
    {

        if (timer == 10)
        {
            var chunksToDelete = new List<WorldPosition>();
            foreach (var chunk in world.chunks)
            {
                float distance = Vector3.Distance(
                    new Vector3(chunk.Value.pos.x, 0, chunk.Value.pos.z),
                    new Vector3(transform.position.x, 0, transform.position.z));

                if (distance > 256)
                    chunksToDelete.Add(chunk.Key);
            }

            foreach (var chunk in chunksToDelete)
                world.DestroyChunk(chunk.x, chunk.y, chunk.z);

            timer = 0;
            return true;
        }

        timer++;
        return false;
    }
}