using Utilities.Extensions;

namespace Year2020;

public class Day6
{
    [Theory]
    [FileLines("data_sample.txt", 11)]
    [FileLines("data.txt", 6703)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var answers = data
            .ChunkBy("")
            .Select(g => g
                .SelectMany(_ => _)
                .Distinct()
                .Count())
            .Sum();

        Assert.Equal(result, answers);
    }

    [Theory]
    [FileLines("data_sample.txt", 6)]
    [FileLines("data.txt", 3430)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var answers = data
            .ChunkBy("")
            .Select(g => g
                .SelectMany(_ => _)
                .GroupBy(_ => _)
                .Count(gr => gr.Count() == g.Length)
            )
            .Sum();

        Assert.Equal(result, answers);
    }
}
