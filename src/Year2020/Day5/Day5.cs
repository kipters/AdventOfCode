using Utilities.Extensions;

namespace Year2020;

public class Day5
{
    [Theory]
    [InlineData(new string[] { "BFFFBBFRRR" }, 567)]
    [InlineData(new string[] { "FFFBBBFRRR" }, 119)]
    [InlineData(new string[] { "BBFFBBFRLL" }, 820)]
    [InlineData(new string[] { "FFFBBBFRRR", "BBFFBBFRLL" }, 820)]
    [FileLines("data.txt", 906)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var highest = data
            .Select(ParseSeatId)
            .Max();

        Assert.Equal(result, highest);
    }

    private static int ParseSeatId(string seatString) => seatString.Aggregate(0, (a, c) => c switch
    {
        'B' => (a << 1) | 1,
        'R' => (a << 1) | 1,
        _ => a << 1
    });

    [Theory]
    [FileLines("data.txt", 519)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var mySeat = data
            .Select(ParseSeatId)
            .Order()
            .SlidingWindow(2)
            .Single(w => w[1] - w[0] == 2);

        Assert.Equal(result, mySeat[0] + 1);
    }
}
