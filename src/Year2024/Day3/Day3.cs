using System.Text.RegularExpressions;

namespace Year2024;

public partial class Day3
{
    [Theory]
    [FileData("data_sample.txt", 161)]
    [FileData("data.txt", 179571322)]
    public void Part1(string data, int result)
    {
        var regex = MulRegex();

        var sum = regex
            .Matches(data)
            .Select(m => (a: int.Parse(m.Groups[1].Value), b: int.Parse(m.Groups[2].Value)))
            .Sum(t => t.a * t.b);

        Assert.Equal(result, sum);
    }

    [Theory]
    [FileData("data_sample2.txt", 48)]
    [FileData("data.txt", 103811193)]
    public void Part2(string data, int result)
    {
        var regex = FullRegex();

        var sum = regex
            .Matches(data)
            .Aggregate((acc: 0, enabled: true), (s, m) =>
            {
                var (acc, enabled) = s;
                var op = (m.Groups[1].Value, m.Groups[4].Value, m.Groups[5].Value);
                return op switch
                {
                    ("mul", "", "") when enabled => (acc: acc + int.Parse(m.Groups[2].Value) * int.Parse(m.Groups[3].Value), enabled: true),
                    ("mul", "", "") => s,
                    ("", "do", "") => (acc, enabled: true),
                    ("", "", "don't") => (acc, enabled: false),
                    _ => throw new InvalidOperationException()
                };
            }, s => s.acc);

        Assert.Equal(result, sum);
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"(mul)\((\d{1,3}),(\d{1,3})\)|(do)\(\)|(don\'t)\(\)", RegexOptions.Multiline)]
    private static partial Regex FullRegex();
}
