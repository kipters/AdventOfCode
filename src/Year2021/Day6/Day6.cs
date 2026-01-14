namespace Year2021;

public class Day6
{
    [Theory]
    [InlineData("3,4,3,1,2", 5934)]
    [FileData("data.txt", 358214)]
    public void Part1(string data, int result)
    {
        var startingPopulation = data
            .Split(',')
            .Select(byte.Parse)
            .ToList();

        var finalPopulation = Enumerable
            .Range(0, 80)
            .Aggregate(startingPopulation, (p, _) =>
            {
                var px = p.Select(f => f switch
                {
                    0 => [6, 8],
                    _ => new byte[] { (byte)(f - 1) }
                })
                .SelectMany(_ => _)
                .ToList();

                var matured = px.Count(n => n == 8);
                return px;
            })
            .Count;

        Assert.Equal(result, finalPopulation);
    }

    [Theory]
    [InlineData("3,4,3,1,2", 26984457539L)]
    [FileData("data.txt", 1622533344325L)]
    public void Part2(string data, long result)
    {
        var startingPopulation = data
            .Split(',')
            .Select(byte.Parse)
            .ToList();

        var growthRates = Enumerable
            .Range(0, 7)
            .Select(x => (long) startingPopulation.Count(i => i == x))
            .ToArray();

        var teens = new long[9];

        var population = (long) startingPopulation.Count;

        for (var i = 0; i < 256; i++)
        {
            var dayOfWeek = i % 7;
            var dayOfMaturationCycle = i % 9;

            var newBorns = growthRates[dayOfWeek];
            var firstBorns = teens[dayOfMaturationCycle];
            teens[dayOfMaturationCycle] = newBorns + firstBorns;
            growthRates[dayOfWeek] += firstBorns;

            population += newBorns;
            population += firstBorns;
        }

        Assert.Equal(result, population);
    }
}
