using Utilities.Extensions;
using Point = (int x, int y);

namespace Year2024;

public class Day8
{
    [Theory]
    [FileLines("data_sample.txt", 14)]
    [FileLines("data.txt", 329)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var d = data.ToArray();
        var maxX = d[0].Length;
        var maxY = d.Length;
        var antinodes = d
            .SelectMany(FindAntennas)
            .GroupBy(t => t.id, t => (t.x, t.y))
            .SelectMany(FindAntinodes)
            .Where(p => p.x >= 0 && p.x < maxX && p.y >= 0 && p.y < maxY)
            .Distinct()
            .Count();

        Assert.Equal(result, antinodes);
    }

    private static IEnumerable<Point> FindAntinodes(IGrouping<char, Point> grouping) => grouping.CartesianProduct().Where(t => t.a != t.b).SelectMany(GetAntinodes);
    private static IEnumerable<Point> GetAntinodes((Point a, Point b) antennas)
    {
        var (a, b) = antennas;
        var (dx, dy) = (a.x - b.x, a.y - b.y);

        return [(a.x + dx, a.y + dy), (b.x - dx, b.y - dy)];
    }

    private static IEnumerable<(char id, int x, int y)> FindAntennas(string line, int y) =>
        line.Select((c, x) => (id: c, x, y)).Where(c => c.id != '.');

    [Theory]
    [FileLines("data_sample.txt", 34)]
    [FileLines("data.txt", 1190)]
    // This may be one of the worst pieces of code I've ever written - all laziness driven
    public void Part2(IEnumerable<string> data, int result)
    {
        var d = data.ToArray();
        var maxX = d[0].Length;
        var maxY = d.Length;
        var antinodes = d
            .SelectMany(FindAntennas)
            .GroupBy(t => t.id, t => (t.x, t.y))
            .SelectMany(FindResonantAntinodes)
            .Where(p => p.x >= 0 && p.x < maxX && p.y >= 0 && p.y < maxY)
            .Distinct()
            .Count();

        Assert.Equal(result, antinodes);
    }

    private static IEnumerable<Point> FindResonantAntinodes(IGrouping<char, Point> grouping) => grouping
        .CartesianProduct()
        .Where(t => t.a != t.b)
        .SelectMany(GetResonantAntinodes);

    private static IEnumerable<Point> GetResonantAntinodes((Point a, Point b) antennas)
    {
        var (a, b) = antennas;
        var (dx, dy) = (a.x - b.x, a.y - b.y);

        var plus = 0.CountTo(50).Select(i => (a.x + i * dx, a.y + i * dy));
        var minus = 0.CountTo(50).Select(i => (b.x - i * dx, b.y - i * dy));
        return plus.Concat(minus);
    }
}
