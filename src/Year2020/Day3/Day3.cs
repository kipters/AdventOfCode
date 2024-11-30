using Utilities.Extensions;

namespace Year2020;

public class Day3
{
    [Theory]
    [FileLines("data_sample.txt", 7)]
    [FileLines("data.txt", 162)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var trees = CountTrees(data, 3, 1);

        Assert.Equal(result, trees);
    }

    private static int CountTrees(IEnumerable<string> data, int stride, int down)
    {
        return data
            .Chunk(down)
            .Select(w => w[0])
            .Select((l, i) => l[i * stride % l.Length])
            .Count(c => c == '#');
    }

    [Theory]
    [FileLines("data_sample.txt", 336)]
    [FileLines("data.txt", 3064612320L)]
    public void Part2(IEnumerable<string> data, long result)
    {
        (int right, int down)[] slopes = [(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)];

        var trees = slopes
            .Select(s => (long)CountTrees(data, s.right, s.down))
            .Product();

        Assert.Equal(result, trees);
    }
}
