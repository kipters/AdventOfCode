using Utilities.Extensions;

namespace Year2021;

public class Day1
{
    [Theory]
    [FileLines("data_sample.txt", 7)]
    [FileLines("data.txt", 1139)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var increments = data
            .Select(int.Parse)
            .SlidingWindow(2)
            .Count(w => w[0] < w[1]);

        Assert.Equal(result, increments);
    }

    [Theory]
    [FileLines("data_sample.txt", 5)]
    [FileLines("data.txt", 1103)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var increments = data
            .Select(int.Parse)
            .SlidingWindow(3)
            .Select(Enumerable.Sum)
            .SlidingWindow(2)
            .Count(w => w[0] < w[1]);

        Assert.Equal(result, increments);
    }
}
