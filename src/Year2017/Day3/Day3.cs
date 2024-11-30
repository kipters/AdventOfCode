using System.Numerics;
using Utilities;
using Utilities.Movement;

namespace Year2017;

public class Day3
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(12, 3)]
    [InlineData(23, 2)]
    [InlineData(1024, 31)]
    [InlineData(277678, 475)]
    public void Part1(int data, int result)
    {
        var coordinates = GetSpiralCoordinates(data);
        var steps = Math.Abs(coordinates.X) + Math.Abs(coordinates.Y);

        Assert.Equal(result, steps);
    }

    private static Vector2 GetSpiralCoordinates(int index) => WalkSpiral().Skip(index - 1).First();

    private static IEnumerable<Vector2> WalkSpiral()
    {
        var plane = new CartesianPlane(Vector2.Zero, Direction.South);

        yield return plane.Position;
        var steps = 1;

        for(;;)
        {
            plane.Turn(TurnDirection.Left);

            for (var i = 0; i < steps; i++)
            {
                plane.Walk(1);
                yield return plane.Position;
            }

            plane.Turn(TurnDirection.Left);

            for (var i = 0; i < steps; i++)
            {
                plane.Walk(1);
                yield return plane.Position;
            }

            steps++;
        }
    }

    [Theory]
    [InlineData(277678, 279138)]
    public void Part2(int data, int result)
    {
        var cells = new Dictionary<Vector2, int>
        {
            [Vector2.Zero] = 1
        };

        var output = 0;

        foreach (var square in WalkSpiral().Skip(1))
        {
            var value = 0;

            value += cells.TryGetValue(new(square.X - 1, square.Y + 1), out var v) ? v : 0;
            value += cells.TryGetValue(new(square.X,     square.Y + 1), out v) ? v : 0;
            value += cells.TryGetValue(new(square.X + 1, square.Y + 1), out v) ? v : 0;

            value += cells.TryGetValue(new(square.X - 1, square.Y    ), out v) ? v : 0;
            value += cells.TryGetValue(new(square.X + 1, square.Y    ), out v) ? v : 0;

            value += cells.TryGetValue(new(square.X - 1, square.Y - 1), out v) ? v : 0;
            value += cells.TryGetValue(new(square.X,     square.Y - 1), out v) ? v : 0;
            value += cells.TryGetValue(new(square.X + 1, square.Y - 1), out v) ? v : 0;

            if (value > data)
            {
                output = value;
                break;
            }

            cells[square] = value;
        }

        Assert.Equal(result, output);
    }
}
