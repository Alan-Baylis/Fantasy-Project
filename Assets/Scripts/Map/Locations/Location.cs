using System;
using UnityEngine;

[Serializable]
//Location represents all positions of WorldObjects in a given world
public class Location {

    //The world that these locations map for
    private World world;

    private Vector3 position;

    //Initialise the location at the world and the x, y, z coordinates
    public Location(World world, float x, float y, float z) {

        this.world = world;
        this.position = new Vector3(x, y, z);

    }

    //Do the same but with a given Vector3, where all float coordinates have to be rounded to integers first
    public Location(World world, Vector3 position) {

        this.world = world;
        this.position = position;

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

    public Vector3 getPosition() {
        return this.position;
    }

    public void setPosition(Vector3 position) {
        this.position = position;
    }

    //Get the x coordinate and set it below:
    public virtual float getX() {
        return this.position.x;
    }

    public virtual int getBlockX() {
        return Mathf.FloorToInt(getX());
    }

    public virtual void setX(float x) {
        this.position.x = x;
    }

    //same for y:
    public virtual float getY() {
        return this.position.y;
    }

    public virtual int getBlockY() {
        return Mathf.FloorToInt(getY());
    }

    public virtual void setY(float y) {
        this.position.y = y;
    }

    //and z:
    public virtual float getZ() {
        return this.position.z;
    }

    public virtual int getBlockZ() {
        return Mathf.FloorToInt(getZ());
    }

    public virtual void setZ(float z) {
        this.position.z = z;
    }

    //Set all components of the location in one go, just call all the functions above for the required values
    public virtual void setLocation(Location location) {

        setWorld(location.getWorld());
        setPosition(location.getPosition()); 

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

        Vector3 positionSum = location1.getPosition() + location2.getPosition();
        Location sum = new Location(world, positionSum);

        return sum;
    }

    //Adding a Vector3 to a location adds the vector to the coordinates of the location
    public static Location operator +(Location location, Vector3 position) {

        World world = location.getWorld();

        Vector3 positionSum = location.getPosition() + position;
        Location sum = new Location(world, positionSum);

        return sum;
    }

    //Add functionality that allows two locations to be subtracted from each other to get a new location
    public static Location operator -(Location location1, Location location2) {

        if(location1.getWorld() != location2.getWorld()) {
            throw new ArgumentException("You cannot subtract Locations from different worlds");
        }
        World world = location1.getWorld();

        Vector3 positionSum = location1.getPosition() - location2.getPosition();
        Location sum = new Location(world, positionSum);

        return sum;
    }

    //Subtracting a Vector3 from a location subtracts the vector from the coordinates of the location
    public static Location operator -(Location location, Vector3 position) {

        World world = location.getWorld();

        Vector3 positionSum = location.getPosition() - position;
        Location sum = new Location(world, positionSum);

        return sum;
    }

    //Add functionality that allows two locations to be subtracted from each other to get a new location
    public static Location operator *(Location location, float scaleFactor) {

        World world = location.getWorld();

        Vector3 positionSum = location.getPosition() * scaleFactor;
        location = new Location(world, positionSum);

        return location;
    }

    public static bool operator ==(Location location, object obj) {

        return location.Equals(obj);

    }

    public static bool operator !=(Location location, object obj) {
        return !(location == obj);
    }

    //Returns a unique number identifying the coordinate
    public override int GetHashCode() {
        unchecked {

            int hash = 47;

            hash *= 227 + this.getX().GetHashCode();
            hash *= 227 + this.getY().GetHashCode();
            hash *= 227 + this.getZ().GetHashCode();

            hash += this.getWorld().GetHashCode();

            return hash;
        }
    }

    public override bool Equals(object obj) {

        if(!(obj is Location)) {
            return false;
        } else {

            Location location = (Location) obj;

            World world = location.getWorld();
            if(world != getWorld()) {
                return false;
            }

            return location.getPosition() == this.getPosition();

        }

    }

    public double distance(Vector3 position) {

        return Vector3.Distance(getPosition(), position);

    }

    public double distance(Location location) {
        return distance(location.getPosition());
    }

    public override string ToString() {

        return "Location @ World: " + getWorld() + " (" + getX() + ", " + getY() + ", " + getZ() + ")"; 

    }

}
