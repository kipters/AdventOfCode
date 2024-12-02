using Utilities.Extensions;

namespace Year2024;

public class Day2
{
    [Theory]
    [FileLines("data_sample.txt", 2)]
    [FileLines("data.txt", 252)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var safe = data.Count(IsReportSafe);

        Assert.Equal(result, safe);
    }

    [Theory]
    [InlineData("7 6 4 2 1", true)]
    [InlineData("1 2 7 8 9", false)]
    [InlineData("9 7 6 2 1", false)]
    [InlineData("1 3 2 4 5", false)]
    [InlineData("8 6 4 4 1", false)]
    [InlineData("1 3 6 7 9", true)]
    public void CanEstimateSafety(string report, bool isSafe) => Assert.Equal(isSafe, IsReportSafe(report));

    private static bool IsReportSafe(string report) => AreLevelsSafe(report
        .Split(' ')
        .Select(int.Parse));

    private static bool AreLevelsSafe(IEnumerable<int> levels) => levels
        .SlidingWindow(2)
            .Aggregate(
                (increase: 0, decrease: 0, deltafail: 0),
                (s, w) => (w[0] - w[1]) switch
                {
                    >3 => s with { deltafail = s.deltafail + 1 },
                    >0 => s with { decrease = s.decrease + 1},
                    0 => s with { deltafail = s.deltafail + 1 },
                    <-3 => s with { deltafail = s.deltafail + 1 },
                    <0 => s with { increase = s.increase + 1 },
                }, s => s switch
                {
                    (_, 0, 0) => true,
                    (0, _, 0) => true,
                    _ => false
                });

    [Theory]
    [FileLines("data_sample.txt", 4)]
    [FileLines("data.txt", 324)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var safe = data.Count(IsReportSafeWithDampener);

        Assert.Equal(result, safe);
    }

    [Theory]
    [InlineData("7 6 4 2 1", true)]
    [InlineData("1 2 7 8 9", false)]
    [InlineData("9 7 6 2 1", false)]
    [InlineData("1 3 2 4 5", true)]
    [InlineData("8 6 4 4 1", true)]
    [InlineData("1 3 6 7 9", true)]
    public void CanEstimateSafetyWithDampener(string report, bool isSafe) => Assert.Equal(isSafe, IsReportSafeWithDampener(report));

    private static bool IsReportSafeWithDampener(string report)
    {
        var originalReport = report
            .Split(' ')
            .Select(int.Parse)
            .ToArray();

        return
            AreLevelsSafe(originalReport) ||
            Enumerable
                .Range(0, originalReport.Length - 1)
                .Select(i => originalReport[0..i].Concat(originalReport[(i+1)..]))
                .Any(AreLevelsSafe) ||
            AreLevelsSafe(originalReport[..^1]);
    }
}
