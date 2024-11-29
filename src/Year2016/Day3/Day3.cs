namespace Year2016;

public class Day3
{
    [Theory]
    [FileLines("data.txt", 862)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var count = data
            .Select(l => l
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray()
            )
            .Count(IsValid);

        Assert.Equal(result, count);
    }

    [Theory]
    [FileLines("data.txt", 1577)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var count = data
            .Select(l => l
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(int.Parse)
                .ToArray()
            )
            .Chunk(3)
            .SelectMany(c => new bool[]
            {
                IsValid(c[0][0], c[1][0], c[2][0]),
                IsValid(c[0][1], c[1][1], c[2][1]),
                IsValid(c[0][2], c[1][2], c[2][2]),
            })
            .Count(_ => _);

        Assert.Equal(result, count);
    }

    private static bool IsValid(int[] triangle) => IsValid(triangle[0], triangle[1], triangle[2]);
    private static bool IsValid(int a, int b, int c) => a + b > c && a + c > b && b + c > a;
}
