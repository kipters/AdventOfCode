using Utilities.Extensions;

namespace Year2020;

public class Day9
{
    [Theory]
    [FileLines("data_sample.txt", 5, 127L)]
    [FileLines("data.txt", 25, 1721308972L)]
    public void Part1(IEnumerable<string> data, int preambleSize, long result)
    {
        var notSum = data
            .Select(long.Parse)
            .SlidingWindow(preambleSize + 1)
            .First(IsNotSum);

        Assert.Equal(result, notSum[^1]);
    }

    private bool IsNotSum(long[] arg) => !arg[..^1]
        .CartesianProduct()
        .Select(p => p.a + p.b)
        .Contains(arg[^1]);

    [Theory]
    [FileLines("data_sample.txt", 5, 62L)]
    [FileLines("data.txt", 25, 209694133L)]
    public void Part2(IEnumerable<string> data, int preambleSize, long result)
    {
        var numData = data
            .Select(long.Parse)
            .ToArray();

        var notSum = numData
            .SlidingWindow(preambleSize + 1)
            .First(IsNotSum);

        var weakness = Enumerable.Range(2, 1000)
            .SelectMany(w => FindSumSet(numData, w, notSum[^1]))
            .Select(s => s.Bounds())
            .Select(s => s.min + s.max)
            .First()
            ;

        Assert.Equal(result, weakness);
    }

    private static IEnumerable<long[]> FindSumSet(IEnumerable<long> data, int windowSize, long sum)
    {
        return data
            .SlidingWindow(windowSize)
            .Where(w => w.Sum() == sum);
    }
}
