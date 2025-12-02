using System.Text.RegularExpressions;
using Utilities.Comparers;
using Utilities.Extensions;

namespace Year2025;

public partial class Day2
{
    [Theory]
    [FileData("data_sample.txt", 1227775554L)]
    [FileData("data.txt", 30599400849L)]
    public void Part1(string data, long result)
    {
        var invalidSum = ParseRanges()
            .Matches(data)
            .Select(m => (
                start: long.Parse(m.Groups[2].Value),
                end: long.Parse(m.Groups[3].Value)
                ))
            .SelectMany(t => t.start.CountTo(t.end))
            .Where(IsInvalidProductId)
            .Sum();

        Assert.Equal(result, invalidSum);

        static bool IsInvalidProductId(long pid)
        {
            var str = pid.ToString();
            if (str.Length % 2 == 1)
            {
                return false;
            }

            var half = str.Length / 2;

            return str[0..half] == str[half..];
        }
    }


    [Theory]
    [FileData("data_sample.txt", 4174379265L)]
    [FileData("data.txt", 46270373595L)]
    public void Part2(string data, long result)
    {
        var invalidSum = ParseRanges()
            .Matches(data)
            .Select(m => (
                start: long.Parse(m.Groups[2].Value),
                end: long.Parse(m.Groups[3].Value)
                ))
            .SelectMany(t => t.start.CountTo(t.end))
            .Where(IsInvalidProductId)
            .Sum();

        Assert.Equal(result, invalidSum);

        static bool IsInvalidProductId(long pid)
        {
            var str = pid.ToString();
            var comparer = new ArrayEqualityComparer<char>();

            return 1.CountTo(str.Length - 1)
                .Where(l => str.Length % l == 0)
                .Any(l => str.Chunk(l).AllEqual(comparer));
        }
    }

    [GeneratedRegex(@"((\d+)-(\d+))")]
    private static partial Regex ParseRanges();
}


