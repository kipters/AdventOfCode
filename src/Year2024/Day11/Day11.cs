using Utilities.Extensions;

namespace Year2024;

public class Day11
{
    [Theory]
    [InlineData("125 17", 55312)]
    [InlineData("4 4841539 66 5279 49207 134 609568 0", 212655)]
    public void Part1(string data, int result)
    {
        var stones = ParseStones(data);
        var count = CountGroupedSteps(stones, 25);

        Assert.Equal(result, count);
    }

    [Theory]
    [InlineData("4 4841539 66 5279 49207 134 609568 0", 253582809724830)]
    public void Part2(string data, long result)
    {
        var stones = ParseStones(data);
        var count = CountGroupedSteps(stones, 75);

        Assert.Equal(result, count);
    }

    private static Dictionary<long, long> ParseStones(string data) => data
        .Split(' ')
        .Select(long.Parse)
        .GroupBy(_ => _)
        .ToDictionary(g => g.Key, g => g.LongCount());

    private static long CountGroupedSteps(Dictionary<long, long> stones, int steps)
    {
        for (var i = 0; i < steps; i++)
        {
            var upd = new Dictionary<long, long>();

            foreach (var (stone, qty) in stones)
            {
                if (stone == 0)
                {
                    upd.SetOrIncrement(1, qty);
                    continue;
                }

                var str = stone.ToString();
                if (str.Length % 2 == 0)
                {
                    var half = str.Length / 2;
                    var s1 = str[..half];
                    var s2 = str[half..];

                    upd.SetOrIncrement(long.Parse(s1), qty);
                    upd.SetOrIncrement(long.Parse(s2), qty);
                    continue;
                }

                upd.SetOrIncrement(stone * 2024, qty);
            }

            stones = upd;
        }

        var count = stones.Sum(kvp => kvp.Value);
        return count;
    }
}
