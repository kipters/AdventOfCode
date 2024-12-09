using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Year2016;

public partial class Day7
{
    [Theory]
    [InlineData(new[] { "abba[mnop]qrst" }, 1)]
    [InlineData(new[] { "abcd[bddb]xyyx" }, 0)]
    [InlineData(new[] { "aaaa[qwer]tyui" }, 0)]
    [InlineData(new[] { "ioxxoj[asdfgh]zxcvbn" }, 1)]
    [FileLines("data.txt", 118)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var regex = SplitRegex();
        var tlsSupported = data
            .Select(l => regex.Split(l)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray()
            )
            .Count(SupportsTls);

        Assert.Equal(result, tlsSupported);
    }

    private static bool SupportsTls(string[] arg) => arg.Aggregate(
        (super: 0, hyper: 0),
        (acc, p) => p switch
        {
            [ '[', .., ']' ] => FindQuads(p).Any() ? acc with { hyper = acc.hyper + 1 } : acc,
            _ => FindQuads(p).Any() ? acc with { super = acc.super + 1 } : acc
        },
        a => a.super > 0 && a.hyper == 0
    );

    private static IEnumerable<char[]> FindQuads(string part) => part
        .SlidingWindow(4)
        .Where(w => w.Length == 4)
        .Where(w => w[0] == w[3] && w[1] == w[2] && w[0] != w[1]);

    [Theory]
    // [InlineData(new[] { "aba[bab]xyz" }, 1)]
    // [InlineData(new[] { "xyx[xyx]xyx" }, 0)]
    // [InlineData(new[] { "aaa[kek]eke" }, 1)]
    // [InlineData(new[] { "zazbz[bzb]cdb" }, 1)]
    [FileLines("data.txt", 260)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var regex = SplitRegex();
        var sslSupported = data
            .Select(l => regex.Split(l)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .ToArray()
            )
            .Count(SupportsSsl);

        Assert.Equal(result, sslSupported);
    }

    private bool SupportsSsl(string[] arg)
    {
        var abas = arg
            .Where(p => p is not [ '[', .., ']' ])
            .SelectMany(p => p
                .SlidingWindow(3)
                .Where(w => w.Length == 3)
                .Where(w => w[0] == w[2] && w[0] != w[1])
            )
            .Select(g => (g[0], g[1]));

        var babs = arg
            .Where(p => p is [ '[', .., ']' ])
            .SelectMany(p => p
                .SlidingWindow(3)
                .Where(w => w.Length == 3)
                .Where(w => w[0] == w[2] && w[0] != w[1])
            )
            .Select(g => (g[1], g[0]));

        return abas.Intersect(babs).Any();
    }

    [GeneratedRegex(@"(\[?[a-z]+\]?)")]
    private static partial Regex SplitRegex();
}
