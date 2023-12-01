using System.Numerics;
using Utilities.Movement;

namespace Utilities.Tests.Movement;

public class CartesianPlaneTests
{
    [Fact]
    public void WalkingMovesInTheCurrentDirection()
    {
        var plane = new CartesianPlane(Vector2.Zero, Direction.East);

        plane.Walk(5);

        Assert.Equal(new Vector2(5, 0), plane.Position);
    }

    [Fact]
    public void TurningLeftRotatesCounterclockwise()
    {
        var plane = new CartesianPlane(Vector2.Zero, Direction.West);

        plane.Turn(TurnDirection.Left);

        Assert.Equal(Direction.South, plane.Direction);
    }

    [Fact]
    public void TurningRightRotatesClockwise()
    {
        var plane = new CartesianPlane(Vector2.Zero, Direction.South);

        plane.Turn(TurnDirection.Right);

        Assert.Equal(Direction.West, plane.Direction);
    }
}
