using UnityEngine;
using System.Collections;

public static class EditTerrain
{
    public static WorldPosition GetBlockPos(Vector3 pos)
    {
        WorldPosition blockPos = new WorldPosition(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }

    public static WorldPosition GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );

        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return (float)pos;
    }

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPosition pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }

    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPosition pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }
}