using Utilities.Extensions;

namespace Year2017;

public class Day1
{
    private const string Digits = "0123456789";

    [Theory]
    [InlineData("1122", 3)]
    [InlineData("1111", 4)]
    [InlineData("1234", 0)]
    [InlineData("91212129", 9)]
    [FileData("data.txt", 1223)]
    public static void Part1(string data, int result)
    {
        var acc = data
            .Select(c => Digits.IndexOf(c))
            .Repeat(1)
            .SlidingWindow(2)
            .Where(w => w[0] == w[1])
            .Select(w => w[0])
            .Sum()
            ;

        Assert.Equal(result, acc);
    }

    [Theory]
    [InlineData("1212", 6)]
    [InlineData("1221", 0)]
    [InlineData("123425", 4)]
    [InlineData("123123", 12)]
    [InlineData("12131415", 4)]
    [FileData("data.txt", 1284)]
    public static void Part2(string data, int result)
    {
        var halfsize = data.Length / 2;

        var firstHalf = data.Take(halfsize);
        var secondHalf = data.Skip(halfsize);
        var acc = data
            .Zip(secondHalf.Concat(firstHalf))
            .Where(t => t.First == t.Second)
            .Select(t => Digits.IndexOf(t.First))
            .Sum()
            ;

        Assert.Equal(result, acc);
    }
}
