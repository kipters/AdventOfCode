using System.Drawing;

namespace Year2023;

public class Day3
{

    [Theory]
    [FileLines("data_sample.txt", 4361)]
    [FileLines("data.txt", 527369)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var m = data.ToArray();
        var mbox = new Rectangle(0, 0, m[0].Length, m.Length);
        var numbers = FindNumbers(m)
            .Select(t => (t.n, rect: new Rectangle(t.rect.X - 1, t.rect.Y - 1, t.rect.Width + 2, t.rect.Height + 2)))
            .Select(t =>
            {
                var rect = t.rect;
                rect.Intersect(mbox);
                return (t.n, rect);
            })
            ;

        var symbols = FindSymbols(m).ToArray();

        var sum = numbers
            .Where(n => symbols.Any(s => s.IntersectsWith(n.rect)))
            .Sum(n => n.n)
            ;

        Assert.Equal(result, sum);
    }

    [Theory]
    [FileLines("data_sample.txt", 467835)]
    [FileLines("data.txt", 73074886)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var m = data.ToArray();
        var mbox = new Rectangle(0, 0, m[0].Length, m.Length);
        var numbers = FindNumbers(m)
            .Select(t => (t.n, rect: new Rectangle(t.rect.X - 1, t.rect.Y - 1, t.rect.Width + 2, t.rect.Height + 2)))
            .Select(t =>
            {
                var rect = t.rect;
                rect.Intersect(mbox);
                return (t.n, rect);
            })
            .ToArray()
            ;

        var sum = FindSymbols(m)
            .Select(s => numbers.Where(n => n.rect.IntersectsWith(s)).Take(3).ToArray())
            .Where(n => n.Length == 2)
            .Select(n => n[0].n * n[1].n)
            .Sum();

        Assert.Equal(result, sum);
    }

    private static IEnumerable<Rectangle> FindSymbols(string[] data)
    {
        for (var y = 0; y < data.Length; y++)
        {
            var l = data[y];
            for (var x = 0; x < l.Length; x++)
            {
                var c = l[x];
                if (c != '.' && !char.IsDigit(c))
                {
                    yield return new(x, y, 1, 1);
                }
            }
        }
    }

    private static IEnumerable<(int n, Rectangle rect)> FindNumbers(string[] data)
    {
        for (var y = 0; y < data.Length; y++)
        {
            var l = data[y];
            var isNumber = false;
            var initialX = -1;

            for (var x = 0; x < l.Length; x++)
            {
                var c = l[x];
                var isDigit = char.IsDigit(c);

                if (isDigit)
                {
                    if (!isNumber)
                    {
                        isNumber = true;
                        initialX = x;
                    }
                }
                else
                {
                    if (isNumber)
                    {
                        var rect = new Rectangle(initialX, y, x - initialX, 1);
                        var n = int.Parse(l[initialX..x]);
                        isNumber = false;
                        initialX = -1;
                        yield return (n, rect);
                    }
                }
            }

            if (isNumber)
            {
                var rect = new Rectangle(initialX, y, l.Length - initialX, 1);
                var n = int.Parse(l[initialX..]);
                yield return (n, rect);
            }
        }
    }
}
