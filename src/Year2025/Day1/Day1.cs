using System.Diagnostics;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Year2025;

public partial class Day1
{
    [Theory]
    [FileLines("data_sample.txt", 3)]
    [FileLines("data.txt", 1158)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var password = data
            .Select(line =>
            {
                var n = int.Parse(line[1..]);
                var d = line[0] == 'L' ? -1 : 1;
                return n * d;
            })
            .StatefulSelect(50, (rotation, dial) => (dial + rotation + 100) % 100)
            .Count(s => s == 0);

        Assert.Equal(result, password);
    }

    [Theory]
    [FileLines("data_sample.txt", 6)]
    [FileLines("data.txt", 6860)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var password = data
            .Select(line =>
            {
                var n = int.Parse(line[1..]);
                var d = line[0] == 'L' ? -1 : 1;
                return n * d;
            })
            .Aggregate((dial: 50, count: 0), (s, rotation) =>
            {
                var (dial, count) = s;

                var rotated = dial + rotation;

                var crossovers = rotated <= 0 && dial > 0 ? 1 : 0;

                crossovers += (int)Math.Floor(Math.Abs(rotated) / 100f);

                rotated += 100 * (crossovers + 1);
                rotated %= 100;

                return (rotated, crossovers + count);
            })
            .count;

        Assert.Equal(result, password);
    }

    [GeneratedRegex(@"(L|R)(\d{1,3})")]
    private partial Regex RotationRegex();
}
