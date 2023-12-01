using System.Numerics;

namespace Utilities;

internal static class DirectionVectors
{
    internal static readonly Vector2 North = new(0, +1);
    internal static readonly Vector2 East = new(1, 0);
    internal static readonly Vector2 South = new(0, -1);
    internal static readonly Vector2 West = new(-1, 0);

    internal static readonly Direction[] Compass = [
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West
    ];

    internal static readonly Dictionary<Direction, Vector2> Map = new()
    {
        [Direction.North] = North,
        [Direction.East] = East,
        [Direction.South] = South,
        [Direction.West] = West
    };
}

public enum Direction
{
    North,
    East,
    South,
    West
}

public enum TurnDirection
{
    Left,
    Right
}
