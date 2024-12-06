using System.ComponentModel;
using System.Numerics;

namespace Utilities.Movement;

public readonly record struct ImmutableCartesianPlane(Vector2 Position, Direction Direction = Direction.North)
{
    public ImmutableCartesianPlane Move(Direction direction, int steps)
    {
        Vector2 vector = direction switch
        {
            Direction.North => new(0, -1),
            Direction.East => new(1, 0),
            Direction.South => new(0, 1),
            Direction.West => new(-1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };

        vector *= steps;

        var position = Position + vector;

        return new(position, Direction);
    }

    public ImmutableCartesianPlane Walk(int steps)
    {
        var vector = DirectionVectors.Map[Direction];
        vector *= steps;
        var position = Position + vector;
        return new(position, Direction);
    }

    public ImmutableCartesianPlane Turn(TurnDirection turn)
    {
        var direction = turn switch
        {
            TurnDirection.Left => TurnLeft(Direction),
            TurnDirection.Right => TurnRight(Direction),
            _ => throw new InvalidEnumArgumentException()
        };

        static Direction TurnLeft(Direction initial) => initial switch
        {
            Direction.North => Direction.West,
            Direction.East => Direction.North,
            Direction.South => Direction.East,
            Direction.West => Direction.South,
            _ => throw new InvalidEnumArgumentException()
        };

        static Direction TurnRight(Direction initial) => initial switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new InvalidEnumArgumentException()
        };

        return new(Position, direction);
    }

    public Vector2 LookAhead()
    {
        var vector = DirectionVectors.Map[Direction];
        return Position + vector;
    }

    public static ImmutableCartesianPlane FromPosition(Vector2 position) => new(position);
}
