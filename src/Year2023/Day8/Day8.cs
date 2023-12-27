using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Year2023;

public partial class Day8
{
    [Theory]
    [FileLines("data_sample.txt", 2)]
    [FileLines("data_sample2.txt", 6)]
    [FileLines("data.txt", 12083)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var path = data
            .First()
            .ToCharArray()
            .Loop();

        var regex = RoutingRegex();
        var map = data
            .Skip(2)
            .Select(l => regex.Match(l))
            .ToDictionary(
                m => m.Groups[1].Value,
                m => (
                    left: m.Groups[2].Value,
                    right: m.Groups[3].Value
                )
            );

        var steps = path
            .StatefulSelect("AAA", (i, pos) => Navigate(i, pos, map))
            .TakeWhile(p => p != "ZZZ")
            .Count() + 1;

        Assert.Equal(result, steps);
    }

    private static string Navigate(char i, string pos, Dictionary<string, (string left, string right)> map)
    {
        return i switch
        {
            'L' => map[pos].left,
            'R' => map[pos].right,
            _ => throw new InvalidDataException()
        };
    }

    [Theory]
    [FileLines("data_sample3.txt", 6)]
    [FileLines("data.txt", 13385272668829)]
    public void Part2(IEnumerable<string> data, long result)
    {
        var path = data
            .First()
            .ToCharArray()
            .Loop();

        var regex = RoutingRegex();
        var map = data
            .Skip(2)
            .Select(l => regex.Match(l))
            .ToDictionary(
                m => m.Groups[1].Value,
                m => (
                    left: m.Groups[2].Value,
                    right: m.Groups[3].Value
                )
            );

        var steps = map
            .Keys
            .Where(p => p.EndsWith('A'))
            .Select(p => path
                .StatefulSelect(p, (i, pos) => Navigate(i, pos, map))
                .TakeWhile(r => !r.EndsWith('Z'))
                .LongCount() + 1
            )
            .LeastCommonMultiple();

        Assert.Equal(result, steps);
    }

    [GeneratedRegex(@"(\w*) = \((\w*), (\w*)\)")]
    private static partial Regex RoutingRegex();
}
