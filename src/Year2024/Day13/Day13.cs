using System.Text.RegularExpressions;
using Utilities.Extensions;

using Point = (long x, long y);

namespace Year2024;

public partial class Day13
{
    [Theory]
    [FileData("data_sample.txt", 480)]
    [FileData("data.txt", 29877)]
    public void Part1(string data, int result)
    {
        var cost = ClawMachineRegex()
            .Matches(data)
            .Tap(m =>
            {
                if (m.Groups.Count != 7)
                {
                    throw new Exception();
                }
            })
            .Select(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray())
            .Select(a => new ClawMachine((a[0], a[1]), (a[2], a[3]), (a[4], a[5])))
            .Select(MachineCost)
            .Sum()
            ;

        Assert.Equal(result, cost);
    }

    private static long MachineCost(ClawMachine machine)
    {
        var (a, b, p) = machine;
        var (xa, ya) = a;
        var (xb, yb) = b;
        var (px, py) = p;

        var d = yb * xa - xb * ya;

        var an = (px * yb - xb * py) / d;
        var bn = (py * xa - px * ya) / d;

        return (an * xa + bn * xb) != px || (an * ya + bn * yb) != py
            ? 0
            : 3 * an + bn;
    }

    [Theory]
    [FileData("data_sample.txt", 875318608908)]
    [FileData("data.txt", 99423413811305)]
    public void Part2(string data, long result)
    {
        var cost = ClawMachineRegex()
            .Matches(data)
            .Tap(m =>
            {
                if (m.Groups.Count != 7)
                {
                    throw new Exception();
                }
            })
            .Select(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray())
            .Select(a => new ClawMachine((a[0], a[1]), (a[2], a[3]), (a[4] + 10000000000000, a[5] + 10000000000000)))
            .Select(MachineCost)
            .Sum()
            ;

        Assert.Equal(result, cost);
    }

    [GeneratedRegex(@"Button A: X\+(\d+), Y\+(\d+)\nButton B: X\+(\d+), Y\+(\d+)\nPrize: X=(\d+), Y=(\d+)")]
    private static partial Regex ClawMachineRegex();

    private record struct ClawMachine(Point ButtonA, Point ButtonB, Point Prize);
}
