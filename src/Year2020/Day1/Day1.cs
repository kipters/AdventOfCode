namespace Year2020;

public class Day1
{
    [Theory]
    [FileLines("data_sample.txt", 514579)]
    [FileLines("data.txt", 910539)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var numbers = data
            .Select(int.Parse)
            .ToArray();

        var pair = numbers
            .SelectMany(x => numbers.Select(y => (x, y)))
            .First(t => (t.x + t.y) == 2020);

        var product = pair.x * pair.y;

        Assert.Equal(result, product);
    }

    [Theory]
    [FileLines("data_sample.txt", 241861950)]
    [FileLines("data.txt", 116724144)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var numbers = data
            .Select(int.Parse)
            .ToArray();

        var pairs = numbers
            .SelectMany(x => numbers.Select(y => (x, y)));

        var triplet = pairs
            .SelectMany(t => numbers.Select(z => (t.x, t.y, z)))
            .First(t => (t.x + t.y + t.z) == 2020);

        var product = triplet.x * triplet.y * triplet.z;

        Assert.Equal(result, product);
    }
}
