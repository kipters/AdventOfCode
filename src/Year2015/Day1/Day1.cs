using Utilities.Extensions;

namespace Year2015;

public class Day1
{
    [Theory]
    [InlineData("(())", 0)]
    [InlineData("()()", 0)]
    [InlineData("(((", 3)]
    [InlineData("(()(()(", 3)]
    [InlineData("))(((((", 3)]
    [InlineData("())", -1)]
    [InlineData("))(", -1)]
    [InlineData(")))", -3)]
    [InlineData(")())())", -3)]
    [FileData("data.txt", 74)]
    public static void Part1(string data, int result)
    {
        var floor = data
            .ToCharArray()
            .Select(c => c switch
            {
                '(' => +1,
                ')' => -1,
                _ => 0
            })
            .Sum();

        Assert.Equal(result, floor);
    }

    [Theory]
    [InlineData(")", 1)]
    [InlineData("()())", 5)]
    [FileData("data.txt", 1795)]
    public static void Part2(string data, int result)
    {
        var position = data
            .ToCharArray()
            .Select(c => c switch
            {
                '(' => +1,
                ')' => -1,
                _ => 0
            })
            .ProgressiveSum()
            .TakeWhile(f => f != -1)
            .Count() + 1;

        Assert.Equal(result, position);
    }
}
