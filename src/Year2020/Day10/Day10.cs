namespace Year2020;

public class Day10
{
    [Theory]
    [FileLines("data_sample.txt", 35)]
    [FileLines("data_sample2.txt", 220)]
    [FileLines("data.txt", 1920)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var adapters = data
            .Select(int.Parse)
            .Order()
            .ToList();

        adapters.Add(adapters.Max() + 3);

        var level = 0;
        var ones = 0;
        var threes = 0;

        while (adapters.Count != 0)
        {
            var nextAdapter = adapters.First(a => (a - level) is <= 3 and >0);
            (ones, threes) = (nextAdapter - level) switch
            {
                1 => (ones + 1, threes),
                3 => (ones, threes + 1),
                _ => (ones, threes)
            };
            level = nextAdapter;
            _ = adapters.Remove(nextAdapter);
        }

        Assert.Equal(result, ones * threes);
    }

    [Theory]
    [FileLines("data_sample_small.txt", 3)]
    [FileLines("data_sample.txt", 8)]
    [FileLines("data_sample2.txt", 19208)]
    [FileLines("data.txt", 1511207993344)]
    // For this one I had to look up some hints to solve it in a decent time
    public void Part2(IEnumerable<string> data, long result)
    {
        var adapters = data
            .Select(int.Parse)
            .Order()
            .ToList();

        var maxAdapter = adapters.Max() + 3;
        adapters.Add(maxAdapter);

        var m = new Dictionary<int, long> { [0] = 1 };

        foreach (var a in adapters)
        {
            m[a] = m.GetValueOrDefault(a - 1, 0) + m.GetValueOrDefault(a - 2, 0) + m.GetValueOrDefault(a - 3, 0);
        }

        var combos = m[maxAdapter];

        Assert.Equal(result, combos);
    }
}
