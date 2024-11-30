using System.Text.RegularExpressions;

namespace Year2020;

public partial class Day2
{
    [Theory]
    [FileLines("data_sample.txt", 2)]
    [FileLines("data.txt", 620)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var regex = RuleRegex();

        var valid = data
            .Select(l => regex.Matches(l)[0].Groups)
            .Select(g => (a: int.Parse(g[1].Value), b: int.Parse(g[2].Value), c: g[3].Value[0], pass: g[4].Value))
            .Count(t =>
            {
                var count = t.pass.Count(c => c == t.c);
                return count >= t.a && count <= t.b;
            });

        Assert.Equal(result, valid);
    }

    [Theory]
    [FileLines("data_sample.txt", 1)]
    [FileLines("data.txt", 727)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var regex = RuleRegex();

        var valid = data
            .Select(l => regex.Matches(l)[0].Groups)
            .Select(g => (a: int.Parse(g[1].Value), b: int.Parse(g[2].Value), c: g[3].Value[0], pass: g[4].Value))
            .Count(t =>
            {
                var a = t.pass[t.a - 1];
                var b = t.pass[t.b - 1];

                return a == t.c ^ b == t.c;
            });

        Assert.Equal(result, valid);
    }

    [GeneratedRegex(@"(\d+)-(\d+) (\w+): (\w+)")]
    private static partial Regex RuleRegex();
}
