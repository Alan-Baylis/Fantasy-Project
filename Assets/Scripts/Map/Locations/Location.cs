using System;
using UnityEngine;

[Serializable]
//Location represents all positions of WorldObjects in a given world
public class Location {

    //The world that these locations map for
    public World world;

    //The x, y, z coordinates of this location
    public int x;
    public int y;
    public int z;

    //Initialise the location at the world and the x, y, z coordinates
    public Location(World world, int x, int y, int z) {

        this.world = world;
        this.x = x;
        this.y = y;
        this.z = z;

    }

    //Do the same but with a given Vector3, where all float coordinates have to be rounded to integers first
    public Location(World world, Vector3 position) {

        this.world = world;
        this.x = Mathf.FloorToInt(position.x);
        this.y = Mathf.FloorToInt(position.y);
        this.z = Mathf.FloorToInt(position.z);

    }

    //If a location is given??? just apply it...
    public Location(Location location) {
        setLocation(location);
    }

    //Get the world the location is in, virtual means it can be overriden by a sub class
    public virtual World getWorld() {
        return this.world;
    }

    //Set the world of the location to a new world
    public virtual void setWorld(World world) {
        this.world = world;
    }

    //Get the x coordinate and set it below:
    public virtual int getX() {
        return this.x;
    }

    public virtual void setX(int x) {
        this.x = x;
    }

    //same for y:
    public virtual int getY() {
        return this.y;
    }

    public virtual void setY(int y) {
        this.y = y;
    }

    //and z:
    public virtual int getZ() {
        return this.z;
    }

    public virtual void setZ(int z) {
        this.z = z;
    }

    //Set all components of the location in one go, just call all the functions above for the required values
    public virtual void setLocation(Location location) {

        setWorld(location.getWorld());
        setX(location.getX());
        setY(location.getY());
        setZ(location.getZ());

    }

    //Convert the location to a Vector3 as used by Unity
    public virtual Vector3 asTransform() {

        Vector3 position = new Vector3(x, y, z);
        return position;

    }

    //Get the location beside this location in the given direction
    public Location getNeighbouringLocation(Direction direction) {

        return this + direction.ordinal(getWorld());

    }

    //Add functionality that allows two locations to be added together to get a new location
    public static Location operator +(Location location1, Location location2) {

        if(location1.getWorld() != location2.getWorld()) {
            throw new ArgumentException("You cannot add Locations from different worlds");
        }
        World world = location1.getWorld();

        int x = location1.getX() + location2.getX();
        int y = location1.getY() + location2.getY();
        int z = location1.getZ() + location2.getZ();

        Location sum = new Location(world, x, y, z);
        return sum;
    }

    //Add functionality that allows two locations to be subtracted from each other to get a new location
    public static Location operator -(Location location1, Location location2) {

        if(location1.getWorld() != location2.getWorld()) {
            throw new ArgumentException("You cannot subtract Locations from different worlds");
        }
        World world = location1.getWorld();

        int x = location1.getX() - location2.getX();
        int y = location1.getY() - location2.getY();
        int z = location1.getZ() - location2.getZ();

        Location sum = new Location(world, x, y, z);
        return sum;
    }

}
