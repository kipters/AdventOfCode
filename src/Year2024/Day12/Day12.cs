namespace Year2024;

public class Day12
{
    [Theory]
    [FileLines("data_sample.txt", 140)]
    [FileLines("data_sample2.txt", 772)]
    [FileLines("data_sample3.txt", 1930)]
    [FileLines("data.txt", 1473276)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var m = data.Select(l => l.ToCharArray()).ToArray();
        var width = m[0].Length;
        var height = m.Length;

        var cost = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var c = m[y][x];

                if (char.IsLower(c))
                {
                    continue;
                }

                var (area, perimeter, _) = FillArea(m, x, y, -1, -1);
                cost += area * perimeter;
            }
        }

        Assert.Equal(result, cost);
    }

    private (int area, int perimeter, int corners) FillArea(char[][] m, int x, int y, int srcX, int srcY)
    {
        var width = m[0].Length;
        var height = m.Length;
        var c = m[y][x];
        var mark = char.ToLowerInvariant(c);
        m[y][x] = mark;

        (int x, int y)[] neighbours = [(x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1)];

        var area = 1;
        var perimeter = 0;
        var corners = 0;

        perimeter += x == 0 || (x == width - 1) ? 1 : 0;
        perimeter += y == 0 || (y == height - 1) ? 1 : 0;


        for (var i = 0; i < 4; i++)
        {
            var (nx, ny) = neighbours[i];

            if (nx == -1 || nx == width || ny == -1 || ny == height)
            {
                continue;
            }

            var n = m[ny][nx];

            if ((nx == srcX && ny == srcY) || n == mark)
            {
                continue;
            }

            if (n == c)
            {
                var nr = FillArea(m, nx, ny, x, y);
                area += nr.area;
                perimeter += nr.perimeter;
                corners += nr.corners;
            }
            else
            {
                perimeter++;
            }
        }

        var cellCorners = IsInCorner(-1, -1) + IsInCorner(+1, -1) + IsInCorner(-1, +1) + IsInCorner(+1, +1);

        return (area, perimeter, corners + cellCorners);

        int IsInCorner(int dx, int dy)
        {
            var (tx, ty) = (x + dx, y + dy);
            var h = (tx == -1 || tx == width) ? '§' : m[y][tx];
            var v = (ty == -1 || ty == height) ? '§' : m[ty][x];
            var t = (v == '§' || h == '§') ? '§' : m[ty][tx];

            return (Matches(v), Matches(h), Matches(t)) switch
            {
                (false, false, _) => 1,
                (true, true, false) => 1,
                _ => 0
            };
        }

        bool Matches(char m) => m == mark || m == c;
    }

    [Theory]
    [FileLines("data_sample.txt", 80)]
    [FileLines("data_sample2.txt", 436)]
    [FileLines("data_sample3.txt", 1206)]
    [FileLines("data_sample4.txt", 236)]
    [FileLines("data_sample5.txt", 368)]
    [FileLines("data.txt", 901100)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var m = data.Select(l => l.ToCharArray()).ToArray();
        var width = m[0].Length;
        var height = m.Length;

        var cost = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var c = m[y][x];

                if (char.IsLower(c))
                {
                    continue;
                }

                var (area, _, corners) = FillArea(m, x, y, -1, -1);

                cost += area * corners;
            }
        }

        Assert.Equal(result, cost);
    }
}
