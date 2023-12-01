using Utilities.Geometry;

namespace Year2015;

public class Day2
{
    [Theory]
    [InlineData(new[] { "2x3x4" }, 58)]
    [InlineData(new[] { "1x1x10" }, 43)]
    [FileLines("data.txt", 1588178)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var paper = data
            .Select(ParseBoxSize)
            .Select(t => new Box(t[0], t[1], t[2]))
            .Sum(b => b.Area + b.SideAreas.Min());

        Assert.Equal(result, paper);
    }

    private static int[] ParseBoxSize(string line)
    {
        return line
            .Split('x')
            .Select(int.Parse)
            .ToArray();
    }

    [Theory]
    [InlineData(new[] { "2x3x4" }, 34)]
    [InlineData(new[] { "1x1x10" }, 14)]
    [FileLines("data.txt", 3783758)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var ribbon = data
            .Select(ParseBoxSize)
            .Select(t => new Box(t[0], t[1], t[2]))
            .Sum(b => b.Volume + b.SidePerimeters.Min());

        Assert.Equal(result, ribbon);
    }
}
