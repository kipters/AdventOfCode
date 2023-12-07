using Utilities.Extensions;

namespace Year2023;

public class Day6
{
    [Theory]
    [FileLines("data_sample.txt", 288)]
    [FileLines("data.txt", 131376)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var times = data
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1)
            .Select(int.Parse);

        var distances = data
            .Skip(1)
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1)
            .Select(int.Parse);

        var games = times.Zip(distances, (t, d) => (time: t, record: d));

        var product = games
            .Select(g => Enumerable.Range(0, g.time)
                .Select(t => (g.time - t) * t)
                .Count(x => x > g.record)
            )
            .Product()
            ;

        Assert.Equal(result, product);
    }

    [Theory]
    [FileLines("data_sample.txt", 71503)]
    [FileLines("data.txt", 34123437)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var timeString = data
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1)
            .Aggregate("", (a, x) => a + x, a => a);

        var time = long.Parse(timeString);

        var distanceString = data
            .Skip(1)
            .First()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Skip(1)
            .Aggregate("", (a, x) => a + x, a => a);

        var distance = long.Parse(distanceString);

        var count = EnumerableExtensions.Range(0, time)
            .Select(t => (time - t) * t)
            .Count(x => x > distance);

        Assert.Equal(result, count);
    }
}
