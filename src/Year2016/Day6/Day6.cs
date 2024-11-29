using Utilities.Extensions;

namespace Year2016;

public class Day6
{
    [Theory]
    [FileLines("data_sample.txt", "easter")]
    [FileLines("data.txt", "wkbvmikb")]
    public void Part1(IEnumerable<string> data, string result)
    {
        var len = data.First().Length;
        var accumulator = Enumerable
            .Range(0, len)
            .Select(_ => new List<char>())
            .ToArray();

        var message = data
            .Aggregate(accumulator, (acc, line) =>
            {
                for (var i = 0; i < len; i++)
                {
                    acc[i].Add(line[i]);
                }
                return acc;
            }, acc => acc.Select(l => l.GroupBy(_ => _).OrderByDescending(g => g.Count()).First().Key))
            .ToArray()
            .AsSpan()
            .AsString();

        Assert.Equal(result, message);
    }

    [Theory]
    [FileLines("data_sample.txt", "advent")]
    [FileLines("data.txt", "evakwaga")]
    public void Part2(IEnumerable<string> data, string result)
    {
        var len = data.First().Length;
        var accumulator = Enumerable
            .Range(0, len)
            .Select(_ => new List<char>())
            .ToArray();

        var message = data
            .Aggregate(accumulator, (acc, line) =>
            {
                for (var i = 0; i < len; i++)
                {
                    acc[i].Add(line[i]);
                }
                return acc;
            }, acc => acc.Select(l => l.GroupBy(_ => _).OrderBy(g => g.Count()).First().Key))
            .ToArray()
            .AsSpan()
            .AsString();

        Assert.Equal(result, message);

    }
}
