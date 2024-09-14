using Utilities.Extensions;

namespace Year2016;

public class Day2
{
    [Theory]
    [FileLines("data_sample.txt", 1985)]
    [FileLines("data.txt", 65556)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var keypad = (x: 1, y: 1);
        var pin = data.StatefulSelect(keypad, (line, k) =>
        {
            return line.Aggregate(k, (x, d) =>
            {
                var (dx, dy) = d switch
                {
                    'U' => (0, -1),
                    'D' => (0, +1),
                    'L' => (-1, 0),
                    'R' => (+1, 0),
                    _ => throw new IndexOutOfRangeException()
                };

                return (
                    x: Math.Clamp(x.x + dx, 0, 2),
                    y: Math.Clamp(x.y + dy, 0, 2)
                );
            });
        })
        .Select(t => t.y * 3 + t.x + 1)
        .Aggregate(0f, (p, d) => p * 10 + d, pin => (int)pin);

        Assert.Equal(result, pin);
    }

    private static (int x, int y) VectorFromDirection(char direction)
    {
        return direction switch
        {
            'U' => (0, -1),
            'D' => (0, +1),
            'L' => (-1, 0),
            'R' => (+1, 0),
            _ => throw new IndexOutOfRangeException()
        };
    }

    [Theory]
    [FileLines("data_sample.txt", "5DB3")]
    [FileLines("data.txt", "CB779")]
    public void Part2(IEnumerable<string> data, string result)
    {
        var map = new string[]{
            "  1  ",
            " 234 ",
            "56789",
            " ABC ",
            "  D  "
        };
        var keypad = (x: 0, y: 2);
        var pinChars = data.StatefulSelect(keypad, (line, k) =>
        {
            return line.Aggregate(k, (x, d) =>
            {
                var (dx, dy) = VectorFromDirection(d);

                var newLocation = (
                    x: Math.Clamp(x.x + dx, 0, 4),
                    y: Math.Clamp(x.y + dy, 0, 4)
                );

                return map[newLocation.y][newLocation.x] == ' ' ? x : newLocation;
            });
        })
        .Select(t => map[t.y][t.x]);
        var pin = new string(pinChars.ToArray());

        Assert.Equal(result, pin);
    }
}
