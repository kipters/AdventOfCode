using System.Numerics;
using Utilities;
using Utilities.Movement;

namespace Year2016;

public class Day1
{
    private readonly Vector2[] _directions = [
        new Vector2(0, +1),
        new Vector2(+1, 0),
        new Vector2(-1, 0),
        new Vector2(0, -1)
    ];

    [Theory]
    [InlineData("R2, L3", 5)]
    [InlineData("R2, R2, R2", 2)]
    [InlineData("R5, L5, R5, R3", 12)]
    [FileData("data.txt", 246)]
    public static void Part1(string data, int result)
    {
        var plane = new CartesianPlane(Vector2.Zero);

        var instructions = data
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(op => (
                direction: op[0] switch
                {
                    'L' => TurnDirection.Left,
                    'R' => TurnDirection.Right,
                    _ => throw new InvalidDataException()
                },
                steps: int.Parse(op[1..])
            ))
            ;

        foreach (var (direction, steps) in instructions)
        {
            plane.Turn(direction);
            plane.Walk(steps);
        }

        var distance = MathF.Abs(plane.Position.X) + MathF.Abs(plane.Position.Y);

        Assert.Equal(result, distance);
    }

    [Theory]
    [InlineData("R8, R4, R4, R8", 4)]
    [FileData("data.txt", 124)]
    public static void Part2(string data, int result)
    {
        var plane = new CartesianPlane(Vector2.Zero);
        HashSet<Vector2> visited = [];

        var instructions = data
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(op => (
                direction: op[0] switch
                {
                    'L' => TurnDirection.Left,
                    'R' => TurnDirection.Right,
                    _ => throw new InvalidDataException()
                },
                steps: int.Parse(op[1..])
            ))
            ;

        foreach (var (direction, steps) in instructions)
        {
            plane.Turn(direction);
            var found = false;

            for (var i = 0; i < steps; i++)
            {
                plane.Walk(1);
                if (!visited.Add(plane.Position))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                break;
            }
        }

        var distance = MathF.Abs(plane.Position.X) + MathF.Abs(plane.Position.Y);

        Assert.Equal(result, distance);
    }
}
