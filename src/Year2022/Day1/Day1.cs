using Utilities.Extensions;

namespace Year2022;

public class Day1
{
    [Theory]
    [FileLines("data_sample.txt", 24000)]
    [FileLines("data.txt", 71924)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var highestCalories = data
            .Select(l => l.Trim())
            .ChunkBy("")
            .Select(e => e.Sum(int.Parse))
            .Max();

        Assert.Equal(result, highestCalories);
    }

    [Theory]
    [FileLines("data_sample.txt", 45000)]
    [FileLines("data.txt", 210406)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var highestCalories = data
            .Select(l => l.Trim())
            .ChunkBy("")
            .Select(e => e.Sum(int.Parse))
            .OrderDescending()
            .Take(3)
            .Sum();

        Assert.Equal(result, highestCalories);
    }
}
