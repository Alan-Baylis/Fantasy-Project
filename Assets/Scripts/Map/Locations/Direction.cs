//Direction descibes the positions a WorldObject can face, they are the cardinal compass directions and then an unknown
public enum Direction {

    NORTH,
    SOUTH,
    EAST,
    WEST,
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
            default:
                return Direction.NORTH;
        }

    }

    //return the Direction's location as a location of size one
    public static Location ordinal(this Direction direction, World world) {

        switch(direction) {
            case Direction.NORTH:
                return new Location(world, 0, 1, 0);
            case Direction.SOUTH:
                return new Location(world, 0, -1, 0);
            case Direction.EAST:
                return new Location(world, -1, 0, 0);
            case Direction.WEST:
                return new Location(world, 1, 0, 0);
            default:
                return new Location(world, 0, 0, 0);
        }

    }

}
