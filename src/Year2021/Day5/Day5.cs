using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Geometry;

namespace Year2021;

public partial class Day5
{
    [Theory]
    [FileData("data_sample.txt", 5)]
    [FileData("data.txt", 7436)]
    public void Part1(string data, int result)
    {
        var dangerousPoints = VentsRegex()
            .Matches(data)
            .Select(m =>
            {
                var ax = int.Parse(m.Groups[1].Value);
                var ay = int.Parse(m.Groups[2].Value);
                var bx = int.Parse(m.Groups[3].Value);
                var by = int.Parse(m.Groups[4].Value);

                return new Line(new(ax,ay), new(bx,by));
            })
            .Where(l => l.Horizontal ^ l.Vertical)
            .SelectMany(l => l.Points())
            .ToFrequencies()
            .Count(p => p.Value > 1);

        Assert.Equal(result, dangerousPoints);
    }

    [Theory]
    [FileData("data_sample.txt", 12)]
    [FileData("data.txt", 21104)]
    public void Part2(string data, int result)
    {
        var dangerousPoints = VentsRegex()
            .Matches(data)
            .Select(m =>
            {
                var ax = int.Parse(m.Groups[1].Value);
                var ay = int.Parse(m.Groups[2].Value);
                var bx = int.Parse(m.Groups[3].Value);
                var by = int.Parse(m.Groups[4].Value);

                return new Line(new(ax,ay), new(bx,by));
            })
            .SelectMany(l => l.Points())
            .ToFrequencies()
            .Count(p => p.Value > 1);

        Assert.Equal(result, dangerousPoints);
    }

    [GeneratedRegex(@"(\d+),(\d+) -> (\d+),(\d+)")]
    private static partial Regex VentsRegex();
}
