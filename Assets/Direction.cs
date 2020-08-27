using UnityEngine;

public enum Direction
{
    NORTH,
    EAST,
    SOUTH,
    WEST
}

public static class DirectionExtensionMethods
{
    public static Vector2Int Vector(this Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                return Vector2Int.up;
            case Direction.SOUTH:
                return Vector2Int.down;
            case Direction.EAST:
                return Vector2Int.right;
            case Direction.WEST:
                return Vector2Int.left;
        }
        return Vector2Int.up;
    }

    public static Direction Opposite(this Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                return Direction.SOUTH;
            case Direction.SOUTH:
                return Direction.NORTH;
            case Direction.EAST:
                return Direction.WEST;
            case Direction.WEST:
                return Direction.EAST;
        }
        return Direction.NORTH;
    }
}
