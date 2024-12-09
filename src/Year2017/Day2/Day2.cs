using Utilities.Extensions;
using Xunit.Abstractions;

namespace Year2017;

public class Day2
{
    [Theory]
    [InlineData(new string[] { "5\t1\t9\t5", "7\t5\t3", "2\t4\t6\t8" }, 18)]
    [FileLines("data.txt", 47136)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var checksum = data
            .Select(l => l
                .Split('\t', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
            )
            .Select(NumberEnumerableExtensions.Bounds)
            .Select(t => t.max - t.min)
            .Sum();

        Assert.Equal(result, checksum);
    }

    [Theory]
    [InlineData(new string[] { "5\t9\t2\t8", "9\t4\t7\t3", "3\t8\t6\t5" }, 9)]
    [FileLines("data.txt", 250)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var checksum = data
            .Select(l => l
                .Split('\t', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .CartesianProduct()
                .Where(t => t.x != t.y)
                .Single(t => t.x % t.y == 0)
            )
            .Select(t => t.x / t.y)
            .Sum();

        Assert.Equal(result, checksum);
    }
}
