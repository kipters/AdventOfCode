using System.Text.RegularExpressions;

namespace Year2018;

public partial class Day3
{
    [Theory]
    [FileData("data_sample.txt", 4)]
    [FileData("data.txt", 118858)]
    public void Part1(string data, int result)
    {
        var overclaimed = ClaimRegex()
            .Matches(data)
            .Select(MatchToFabricClaim)
            .Aggregate(
                new ushort[1000, 1000],
                PaintClaim,
                CountOverlappingSquares
            );

        Assert.Equal(result, overclaimed);
    }

    private static int CountOverlappingSquares(ushort[,] acc)
    {
        var count = 0;
        for (var y = 0; y < acc.GetLength(1); y++)
        {
            for (var x = 0; x < acc.GetLength(0); x++)
            {
                if (acc[x, y] > 1)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static ushort[,] PaintClaim(ushort[,] fabric, FabricClaim claim)
    {
        var (_, left, top, width, height) = claim;
        for (var y = top; y < top + height; y++)
        {
            for (var x = left; x < left + width; x++)
            {
                fabric[x, y]++;
            }
        }

        return fabric;
    }

    private static FabricClaim MatchToFabricClaim(Match match)
    {
        var id = int.Parse(match.Groups[1].Value);
        var x = int.Parse(match.Groups[2].Value);
        var y = int.Parse(match.Groups[3].Value);
        var w = int.Parse(match.Groups[4].Value);
        var h = int.Parse(match.Groups[5].Value);
        return new(id, x, y, w, h);
    }

    [Theory]
    [FileData("data_sample.txt", 3)]
    [FileData("data.txt", 1100)]
    public void Part2(string data, int result)
    {
        var claims = ClaimRegex()
            .Matches(data)
            .Select(MatchToFabricClaim)
            .ToArray();

        var claimedFabric = claims
            .Aggregate(new ushort[1000, 1000], PaintClaim);

        var safeClaim = claims
            .Single(c => IsOnlyClaim(claimedFabric, c));

        Assert.Equal(result, safeClaim.Id);
    }

    private static bool IsOnlyClaim(ushort[,] acc, FabricClaim claim)
    {
        var (_, left, top, width, height) = claim;

        for (var y = top; y < top + height; y++)
        {
            for (var x = left; x < left + width; x++)
            {
                if (acc[x, y] > 1)
                {
                    return false;
                }
            }
        }

        return true;
    }

    [GeneratedRegex(@"#(\d+) @ (\d+),(\d+): (\d+)x(\d+)")]
    private static partial Regex ClaimRegex();

    private record FabricClaim(int Id, int Left, int Top, int Width, int Height);
    private record FabricAccumulator(ushort[,] Fabric, List<FabricClaim> NonOverlapping);
}
