using System.Buffers;
using System.Text;
using System.Text.RegularExpressions;

namespace Year2023;

public partial class Day1
{
    [Theory]
    [FileLines("data_sample.txt", 142)]
    [FileLines("data.txt", 55130)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var sv = SearchValues.Create("01234567890".AsSpan());

        var calibration = data
            .Select(l =>
            {
                var s = l.AsSpan();
                var first = s[s.IndexOfAny(sv)];
                var last = s[s.LastIndexOfAny(sv)];
                return int.Parse($"{first}{last}");
            })
            .Sum();

        Assert.Equal(result, calibration);
    }

    [Theory]
    [FileLines("data_sample2.txt", 281)]
    [FileLines("data.txt", 54985)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var frags = new[]
        {
            "zero", "one", "two",
            "three", "four", "five",
            "six", "seven", "eight",
            "nine"
        };

        var regex = NumbersRegex();

        var calibration = data
            .Select(l => regex.Matches(l))
            .Where(m => m.Count != 0)
            .Select(m => (first: Parse(m[0].Groups[1].Value), last: Parse(m[^1].Groups[1].Value)))
            .Select(p => p.first * 10 + p.last)
            .Sum()
            ;

        Assert.Equal(result, calibration);

        int Parse(string frag)
        {
            var res = frag switch
            {
                {Length: 1} => int.Parse(frag),
                _ => Array.IndexOf(frags, frag)
            };

            return res;
        }
    }

    [GeneratedRegex(@"(?=(1|2|3|4|5|6|7|8|9|one|two|three|four|five|six|seven|eight|nine))")]
    private static partial Regex NumbersRegex();
}
