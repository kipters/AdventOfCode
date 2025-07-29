using System.Numerics;
using Utilities.Extensions;

namespace Year2018;

public class Day2
{
    [Theory]
    [FileLines("data_sample.txt", 12)]
    [FileLines("data.txt", 7410)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var checksum = data
            .Select(CountPairsAndTriplets)
            .Aggregate(Vector2.Zero,
                (a, c) => a + c,
                a => a.X * a.Y
            );

        Assert.Equal(result, checksum);
    }

    private Vector2 CountPairsAndTriplets(string line) => line
        .GroupBy(_ => _)
        .Aggregate(
            (pairs: false, triplets: false),
            (a, t) => t.Count() switch
            {
                2 => a with { pairs = true },
                3 => a with { triplets = true },
                _ => a
            },
            a => new Vector2(
                a.pairs ? 1 : 0,
                a.triplets ? 1 : 0
                )
            );

    [Theory]
    [FileLines("data_sample2.txt", "fgij")]
    [FileLines("data.txt", "cnjxoritzhvbosyewrmqhgkul")]
    public void Part2(IEnumerable<string> data, string result)
    {
        var common = data.CartesianProduct()
            .Select(GetDifferences)
            .First(d => d is not null);

        Assert.Equal(result, common);

    }

    private static string? GetDifferences((string, string) tuple)
    {
        var (a, b) = tuple;
        Span<char> buffer = stackalloc char[a.Length];
        var p = 0;
        var d = 0;

        for (var i = 0; i < buffer.Length; i++)
        {
            var ca = a[i];
            var cb = b[i];

            if (ca == cb)
            {
                buffer[p++] = ca;
            }
            else
            {
                d++;
            }
        }

        return d == 1 ? new(buffer[..^1]) : null;
    }
}
