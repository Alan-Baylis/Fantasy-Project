//Direction descibes the positions a WorldObject can face, they are the cardinal compass directions and then an unknown
using UnityEngine;

public enum Direction {

    NORTH,
    SOUTH,
    EAST,
    WEST,
    SELF,
    UP,
    DOWN,
    UNKNOWN

}

//A class of method(s) that can be used with the Direction enum
public static class DirectionMethods {

    //return the opposite face to the current direction
    public static Direction opposite(this Direction direction) {

        switch(direction) {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.EAST:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.EAST;
            case Direction.UP:
                return Direction.DOWN;
            case Direction.DOWN:
                return Direction.UP;
            case Direction.SELF:
                return Direction.SELF;
            default:
                return Direction.NORTH;
        }

    }

    //return the Direction's location as a location of size one
    public static Location ordinal(this Direction direction, World world) {

        switch(direction) {
            case Direction.NORTH:
                return new Location(world, 0, 0, 1);
            case Direction.SOUTH:
                return new Location(world, 0, 0, -1);
            case Direction.EAST:
                return new Location(world, -1, 0, 0);
            case Direction.WEST:
                return new Location(world, 1, 0, 0);
            case Direction.UP:
                return new Location(world, 0, 1, 0);
            case Direction.DOWN:
                return new Location(world, 0, -1, 0);
            default:
                return new Location(world, 0, 0, 0);
        }

    }

    public static Direction getDominantDirection(Vector3 direction) {

        float x = direction.x;
        float y = direction.y;
        float z = direction.z;

        if(x > y && x > z) {
            return Direction.EAST;
        } else if(y > x && y > z) {
            return Direction.UP;
        } else if(z > x && z > y) {
            return Direction.NORTH;
        } else if(x < y && x < z) {
            return Direction.WEST;
        } else if(y < x && y < z) {
            return Direction.DOWN;
        } else if(z < x && z < y) {
            return Direction.SOUTH;
        } else if(x == y && y == z) {
            return Direction.SELF;
        } else {
            return Direction.UNKNOWN;
        }

    }

}
