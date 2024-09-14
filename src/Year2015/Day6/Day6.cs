using System.Text.RegularExpressions;

namespace Year2015;

public partial class Day6
{
    [Theory]
    [InlineData(new[] {"turn on 0,0 through 999,999"}, 1_000_000)]
    [InlineData(new[] {"toggle 0,0 through 999,0"}, 1_000)]
    [InlineData(new[] {"turn off 499,499 through 500,500"}, 0)]
    [FileLines("data.txt", 569999)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var regex = MatchNumber();
        var grid = new bool[1000,1000];
        var lit = 0;
        foreach (var line in data)
        {
            var command = line[..7];

            var coords = regex.Matches(line)
                .Select(m => int.Parse(m.Value))
                .Chunk(2)
                .Select(t => (x: t[0], y: t[1]))
                .ToArray();

            var start = coords[0];
            var end = coords[1];

            for (var y = start.y; y <= end.y; y++)
            {
                for (var x = start.x; x <= end.x; x++)
                {
                    var state = grid[x, y];
                    var bulb = command switch
                    {
                        "turn on" => true,
                        "turn of" => false,
                        "toggle " => !state,
                        _ => throw new IndexOutOfRangeException()
                    };

                    lit += (state, bulb) switch
                    {
                        (false, true) => 1,
                        (true, false) => -1,
                        _ => 0
                    };
                    grid[x, y] = bulb;
                }
            }
        }

        Assert.Equal(result, lit);
    }

    [Theory]
    [InlineData(new[] {"turn on 0,0 through 0,0"}, 1)]
    [InlineData(new[] {"toggle 0,0 through 999,999"}, 2_000_000)]
    [FileLines("data.txt", 17836115)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var regex = MatchNumber();
        var grid = new int[1000,1000];
        var level = 0L;
        foreach (var line in data)
        {
            var command = line[..7];

            var coords = regex.Matches(line)
                .Select(m => int.Parse(m.Value))
                .Chunk(2)
                .Select(t => (x: t[0], y: t[1]))
                .ToArray();

            var start = coords[0];
            var end = coords[1];

            for (var y = start.y; y <= end.y; y++)
            {
                for (var x = start.x; x <= end.x; x++)
                {
                    var state = grid[x, y];
                    var bulb = Math.Max(state + command switch
                    {
                        "turn on" => +1,
                        "turn of" => -1,
                        "toggle " => +2,
                        _ => throw new IndexOutOfRangeException()
                    }, 0);

                    level += bulb - state;
                    grid[x, y] = bulb;
                }
            }
        }

        Assert.Equal(result, level);
    }

    [GeneratedRegex(@"(\d+)")]
    private partial Regex MatchNumber();
}
