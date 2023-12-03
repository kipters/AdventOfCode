using System.ComponentModel;
using System.Numerics;

namespace Utilities.Movement;

public class CartesianPlane
{
    public CartesianPlane(Vector2 position) => Position = position;

    public CartesianPlane(Vector2 position, Direction initialDirection)
    {
        Position = position;
        Direction = initialDirection;
    }

    public Vector2 Position { get; private set; }
    public Direction Direction { get; private set; } = Direction.North;

    public void Move(Direction direction, int steps)
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

        Position += vector;
    }

    public void Walk(int steps)
    {
        var vector = DirectionVectors.Map[Direction];
        vector *= steps;
        Position += vector;
    }

    public void Turn(TurnDirection turn)
    {
        Direction = turn switch
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
    }
}
