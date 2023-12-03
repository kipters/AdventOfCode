using System.Numerics;
using Utilities;
using Utilities.Extensions;
using Utilities.Movement;

namespace Year2015;

public class Day3
{
    [Theory]
    [InlineData(">", 2)]
    [InlineData("^>v<", 4)]
    [InlineData("^v^v^v^v^v", 2)]
    [FileData("data.txt", 2565)]
    public void Part1(string data, int result)
    {
        var houses = data
            .Select(ParseDirections)
            .Mutate(new CartesianPlane(Vector2.Zero),
                (p, d) => p.Move(d, 1),
                p => p.Position
            )
            .Concat(new[] { Vector2.Zero })
            .Distinct()
            .Count()
            ;

        Assert.Equal(result, houses);
    }

    private static Direction ParseDirections(char directionGlyph)
    {
        return directionGlyph switch
        {
            '^' => Direction.North,
            '>' => Direction.East,
            'v' => Direction.South,
            '<' => Direction.West,
            _ => throw new ArgumentOutOfRangeException(nameof(directionGlyph))
        };
    }

    [Theory]
    [InlineData("^v", 3)]
    [InlineData("^>v<", 3)]
    [InlineData("^v^v^v^v^v", 11)]
    [FileData("data.txt", 2639)]
    public void Part2(string data, int result)
    {
        var houses = data
            .Select(ParseDirections)
            .Chunk(2)
            .Mutate(
                (santa: new CartesianPlane(Vector2.Zero), robo: new CartesianPlane(Vector2.Zero)),
                (p, ds) =>
                {
                    var (santa, robo) = p;
                    santa.Move(ds[0], 1);
                    robo.Move(ds[1], 1);
                },
                p => new[] { p.santa.Position, p.robo.Position }
            )
            .SelectMany(_ => _)
            .Concat(new[] { Vector2.Zero })
            .Distinct()
            .Count()
            ;
    }
}
