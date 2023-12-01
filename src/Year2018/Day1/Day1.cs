using Utilities.Extensions;

namespace Year2018;

public class Day1
{
    [Theory]
    [InlineData("+1, -2, +3, +1", 3)]
    [InlineData("+1, +1, +1", 3)]
    [InlineData("+1, +1, -2", 0)]
    [InlineData("-1, -2, -3", -6)]
    [FileData("data.txt", 587)]
    public static void Part1(string data, int result)
    {
        var acc = data
            .Split((char[])['\n', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .Sum();

        Assert.Equal(result, acc);
    }

    [Theory]
    [InlineData("+1, -1", 0)]
    [InlineData("+3, +3, +4, -2, -4", 10)]
    [InlineData("-6, +3, +8, +5, -6", 5)]
    [InlineData("+7, +7, -2, -7, -4", 14)]
    [FileData("data.txt", 83130)]
    public static void Part2(string data, int result)
    {
        var sequence = data
            .Split((char[])['\n', ','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .Loop()
            .ProgressiveSum();

        var seen = new HashSet<int> { 0 };

        var selected = 0;
        foreach (var freq in sequence)
        {
            if (seen.Add(freq))
            {
                continue;
            }

            selected = freq;
            break;
        }

        Assert.Equal(result, selected);
    }
}
