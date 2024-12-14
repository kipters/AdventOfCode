using System.IO.Compression;
using System.Text.RegularExpressions;
using Utilities.Extensions;
using Xunit.Abstractions;

namespace Year2024;

public partial class Day14(ITestOutputHelper io)
{
    [Theory]
    [FileData("data_sample.txt", 12, 11, 7)]
    [FileData("data.txt", 221616000, 101, 103)]
    public void Part1(string data, int result, int width, int height)
    {
        var midX = Math.Floor(width / 2f);
        var midY = Math.Floor(height / 2f);

        var robots = RobotRegex()
            .Matches(data)
            .Assert(m => m.Groups.Count == 5)
            .Select(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray())
            .Select(g => new Robot(g[0], g[1], g[2] + width, g[3] + height))
            .Assert(r => r.Vx >= 0 && r.Vy >= 0)
            .Select(r => r with
            {
                X = (r.X + 100 * r.Vx) % width,
                Y = (r.Y + 100 * r.Vy) % height
            })
            .ToArray();

        io.WriteLine($"{midX} {midY}");

        var topLeft = robots.Count(r => r.X < midX && r.Y < midY);
        var topRight = robots.Count(r => r.X > midX && r.Y < midY);
        var bottomLeft = robots.Count(r => r.X < midX && r.Y > midY);
        var bottomRight = robots.Count(r => r.X > midX && r.Y > midY);

        io.WriteLine($"{topLeft} {topRight} {bottomLeft} {bottomRight}");

        var safetyFactor = topLeft * topRight * bottomLeft * bottomRight;

        for (var y = 0; y < height; y++)
        {
            var c = new char[width];
            if (y == midY)
            {
                io.WriteLine("");
                continue;
            }

            for (var x = 0; x < width; x++)
            {
                if (x == midX)
                {
                    c[x] = ' ';
                    continue;
                }

                var rs = robots.Count(r => r.X == x && r.Y == y);

                c[x] = rs == 0 ? '.' : rs.ToString("x")[0];
            }

            io.WriteLine(new string(c));
        }

        io.WriteLine(safetyFactor.ToString());
        Assert.Equal(result, safetyFactor);
    }

    [Theory]
    [FileData("data_sample.txt", 12, 11, 7)]
    [FileData("data.txt", 221616000, 101, 103)]
    public void Part1Walking(string data, int result, int width, int height)
    {
        var midX = Math.Floor(width / 2f);
        var midY = Math.Floor(height / 2f);

        var robots = RobotRegex()
            .Matches(data)
            .Assert(m => m.Groups.Count == 5)
            .Select(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray())
            .Select(g => new Robot(g[0], g[1], g[2] + width, g[3] + height))
            .Assert(r => r.Vx >= 0 && r.Vy >= 0)
            .ToArray();

        for (var n = 0; n < 100; n++)
        {
            for (var i = 0; i < robots.Length; i++)
            {
                var r = robots[i];

                var x = (r.X + r.Vx) % width;
                var y = (r.Y + r.Vy) % height;
                var key = y * width + x;

                robots[i] = r with { X = x, Y = y };
            }
        }

        io.WriteLine($"{midX} {midY}");

        var topLeft = robots.Count(r => r.X < midX && r.Y < midY);
        var topRight = robots.Count(r => r.X > midX && r.Y < midY);
        var bottomLeft = robots.Count(r => r.X < midX && r.Y > midY);
        var bottomRight = robots.Count(r => r.X > midX && r.Y > midY);

        io.WriteLine($"{topLeft} {topRight} {bottomLeft} {bottomRight}");

        var safetyFactor = topLeft * topRight * bottomLeft * bottomRight;
        Assert.Equal(result, safetyFactor);
    }

    [Theory]
    [FileData("data.txt", 7572, 101, 103)]
    public void Part2(string data, int result, int width, int height)
    {
        var midX = Math.Floor(width / 2f);
        var midY = Math.Floor(height / 2f);

        var robots = RobotRegex()
            .Matches(data)
            .Assert(m => m.Groups.Count == 5)
            .Select(m => m.Groups.Values.Skip(1).Select(g => int.Parse(g.Value)).ToArray())
            .Select(g => new Robot(g[0], g[1], g[2] + width, g[3] + height))
            .Assert(r => r.Vx >= 0 && r.Vy >= 0)
            .ToArray();

        var steps = 0;
        for (var n = 0; n < 10_000; n++)
        {
            var map = new byte[width * height];
            for (var i = 0; i < robots.Length; i++)
            {
                var r = robots[i];

                var x = (r.X + r.Vx) % width;
                var y = (r.Y + r.Vy) % height;
                var key = y * width + x;
                map[key] += 1;
                robots[i] = r with { X = x, Y = y };
            }

            using var ms = new MemoryStream(map);
            using var dms = new MemoryStream();
            using var gzip = new GZipStream(dms, CompressionMode.Compress);

            ms.CopyTo(gzip);
            gzip.Flush();

            // due to integer math, the right compressed array will be
            // smaller enough to be approximated to zero when compared
            // to the original size
            if (dms.Length / robots.Length == 0)
            {
                steps = n + 1;
                break;
            }
        }

        Assert.Equal(result, steps);
    }


    [GeneratedRegex(@"p=(\d+),(\d+) v=(\-?\d+),(\-?\d+)")]
    private static partial Regex RobotRegex();

    private record struct Robot(int X, int Y, int Vx, int Vy);
}
