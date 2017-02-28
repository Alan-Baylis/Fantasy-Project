using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location {

    private int x;
    private int y;
    private int z;

    public Location(int x, int y, int z) {

        this.x = x;
        this.y = y;
        this.z = z;

    }

    public Location(Vector3 position) {

        this.x = Mathf.FloorToInt(position.x);
        this.y = Mathf.FloorToInt(position.y);
        this.z = Mathf.FloorToInt(position.z);

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

    public virtual Vector3 asTransform() {

        Vector3 position = new Vector3(x, y, z);
        return position;

    }

}
