using UnityEngine;
using System.Collections;
using System;

[Serializable]

//Used to describe a coordinate for a block in the world
public struct WorldPosition
{
    public int x, y, z;

    public WorldPosition(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    //A requirement for overriding the equals function, a hashcode is a unique indentifier for the object as a number.
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 47;

            hash = hash * 227 + x.GetHashCode();
            hash = hash * 227 + y.GetHashCode();
            hash = hash * 227 + z.GetHashCode();

            return hash;
        }
    }

    //Allows you to easily compare two world positions
    public override bool Equals(object obj)
    {
        if (GetHashCode() == obj.GetHashCode())
            return true;
        return false;
    }

    //Returns the worldPosition as a Vector3 coordinate
    public Vector3 asTransform() {
        Vector3 position = new Vector3(x, y, z);
        return position;
    }
}