using Utilities.Extensions;

namespace Year2024;

public class Day1
{
    [Theory]
    [FileLines("data_sample.txt", 11)]
    [FileLines("data.txt", 1651298)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var pairs = data
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToArray();

        var left = pairs
            .Select(p => int.Parse(p[0]))
            .Order();

        var right = pairs
            .Select(p => int.Parse(p[1]))
            .Order();

        var totalDistance = left.Zip(right)
            .Sum((t) => Math.Abs(t.First - t.Second));

        Assert.Equal(result, totalDistance);
    }

    [Theory]
    [FileLines("data_sample.txt", 31)]
    [FileLines("data.txt", 21306195)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var pairs = data
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .ToArray();

        var lut = pairs
            .Select(p => int.Parse(p[1]))
            .ToFrequencies();

        var score = pairs
            .Select(p => int.Parse(p[0]))
            .Select(n => lut.TryGetValue(n, out var count) ? n * count : 0)
            .Sum();

        Assert.Equal(result, score);
    }
}
