namespace Year2019;

public class Day1
{
    [Theory]
    [InlineData(new[] { "12" }, 2)]
    [InlineData(new[] { "14" }, 2)]
    [InlineData(new[] { "1969" }, 654)]
    [InlineData(new[] { "100756" }, 33583)]
    [FileLines("data.txt", 3287620)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var fuel = data
            .Select(int.Parse)
            .Sum(n => Math.Floor(n / 3.0) - 2.0);

        Assert.Equal(result, fuel);
    }

    [Theory]
    [InlineData(new[] { "14" }, 2)]
    [InlineData(new[] { "1969" }, 966)]
    [InlineData(new[] { "100756" }, 50346)]
    [FileLines("data.txt", 4928567)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var fuel = data
            .Select(double.Parse)
            .Sum(getFuel);

        Assert.Equal(result, Math.Floor(fuel));

        static double getFuel(double fuelModule)
        {
            var fuelWeight = Math.Floor(fuelModule / 3.0) - 2.0;

            return fuelWeight <= 0 ? 0 : fuelWeight + getFuel(fuelWeight);
        }
    }
}
