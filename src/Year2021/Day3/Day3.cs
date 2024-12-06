using System.Globalization;
using Utilities.Extensions;

namespace Year2021;

public class Day3
{
    [Theory]
    [FileLines("data_sample.txt", 198)]
    [FileLines("data.txt", 3895776)]
    public void Part1(IEnumerable<string> data, uint result)
    {
        var reportLen = data.First().Length;
        var gamma = (uint) data
            .Select(l => l.Select(c => c == '0' ? -1 : 1))
            .Aggregate(
                new int[reportLen].AsEnumerable(),
                (acc, r) => acc.Zip(r, (a, b) => a + b),
                acc => acc.Aggregate(0, (r, b) => b > 0 ? (r << 1) | 1 : (r << 1))
            );

        var mask = (uint) Math.Pow(2, reportLen) - 1;
        var epsilon = ~gamma & mask;

        var powerConsumption = gamma * epsilon;

        Assert.Equal(result, powerConsumption);
    }

    [Theory]
    [FileLines("data_sample.txt", 230)]
    [FileLines("data.txt", 7928162)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var lineLen = data.First().Length;
        var arr = data.ToArray();

        var oxygenRating = Enumerable.Range(0, lineLen)
            .Aggregate(arr, (a, i) => FilterReports(a, i, OxygenRatingCriteria))
            .Single()
            .Then(r => int.Parse(r, NumberStyles.BinaryNumber))
            ;

        var scrubberRating = Enumerable.Range(0, lineLen)
            .Aggregate(arr, (a, i) => FilterReports(a, i, ScrubberRatingCriteria))
            .Single()
            .Then(r => int.Parse(r, NumberStyles.BinaryNumber))
            ;

        var lifeSupportRating = oxygenRating * scrubberRating;
        Assert.Equal(result, lifeSupportRating);
    }

    private static string[] FilterReports(string[] a, int i, Func<(int one, int zero), char> criteria)
    {
        if (a.Length == 1)
        {
            return a;
        }

        var frequencies = a
            .Select(r => r[i])
            .ToFrequencies()
            .Then(d => (one: d['1'], zero: d['0']));

        var needle = criteria(frequencies);

        return a
            .Where(r => r[i] == needle)
            .ToArray();
    }

    private static char OxygenRatingCriteria((int one, int zero) tuple) => tuple switch
    {
        (int one, int zero) when one > zero => '1',
        (int one, int zero) when one < zero => '0',
        (int one, int zero) when one == zero => '1',
        _ => throw new Exception()
    };

    private static char ScrubberRatingCriteria((int one, int zero) tuple) => tuple switch
    {
        (int one, int zero) when one > zero => '0',
        (int one, int zero) when one < zero => '1',
        (int one, int zero) when one == zero => '0',
        _ => throw new Exception()
    };
}
