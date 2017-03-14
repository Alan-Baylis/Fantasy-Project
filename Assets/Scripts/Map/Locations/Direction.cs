
public enum Direction {

    NORTH,
    SOUTH,
    EAST,
    WEST,
    UNKNOWN
    
}

public static class DirectionMethods {

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

}
