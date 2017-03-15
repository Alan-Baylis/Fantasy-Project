using System;
using UnityEngine;

[Serializable]
public class Location {

    public World world;

    public int x;
    public int y;
    public int z;

    public Location(World world, int x, int y, int z) {

        this.x = x;
        this.y = y;
        this.z = z;

    }

    public Location(World world, Vector3 position) {

        this.x = Mathf.FloorToInt(position.x);
        this.y = Mathf.FloorToInt(position.y);
        this.z = Mathf.FloorToInt(position.z);

    }

    public Location(Location location) {
        setLocation(location);
    }

    public virtual World getWorld() {
        return this.world;
    }

    public virtual void setWorld(World world) {
        this.world = world;
    }

    public virtual int getX() {
        return this.x;
    }

    public virtual void setX(int x) {
        this.x = x;
    }

    public virtual int getY() {
        return this.y;
    }

    public virtual void setY(int y) {
        this.y = y;
    }

    public virtual int getZ() {
        return this.z;
    }

    public virtual void setZ(int z) {
        this.z = z;
    }

    public virtual void setLocation(Location location) {

        setWorld(location.getWorld());
        setX(location.getX());
        setY(location.getY());
        setZ(location.getZ());

    }

    public virtual Vector3 asTransform() {

        Vector3 position = new Vector3(x, y, z);
        return position;

    }

}
