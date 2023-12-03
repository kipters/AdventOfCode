using System.Text.RegularExpressions;

namespace Year2023;

public partial class Day2
{
    [Theory]
    [FileLines("data_sample.txt", 8)]
    [FileLines("data.txt", 2285)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var gameRegex = GameRegex();
        var sum = data
            .Select(l => gameRegex.Match(l))
            .Select(m => (id: int.Parse(m.Groups["id"].Value), draws: ParseDraws(m.Groups["draws"].Value)))
            .Where(g => g.draws.All(d => d.Red <= 12 && d.Green <= 13 && d.Blue <= 14))
            .Select(g => g.id)
            .Sum()
            ;

        Assert.Equal(result, sum);
    }



    [Theory]
    [FileLines("data_sample.txt", 2286)]
    [FileLines("data.txt", 77021)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var gameRegex = GameRegex();
        var sum = data
            .Select(l => gameRegex.Match(l))
            .Select(m => ParseDraws(m.Groups["draws"].Value))
            .Select(m => new ColorTriplet(
                Red: m.Max(d => d.Red),
                Green: m.Max(d => d.Green),
                Blue: m.Max(d => d.Blue)
            ))
            .Select(t => t.Red * t.Green * t.Blue)
            .Sum()
            ;

        Assert.Equal(result, sum);
    }

    [GeneratedRegex(@"^Game\s(?<id>\d+):(?<draws>.*)")]
    private static partial Regex GameRegex();

    [GeneratedRegex(@"(?<amount>\d+)\s(?<color>red|green|blue)")]
    private static partial Regex DrawRegex();

    private static IEnumerable<ColorTriplet> ParseDraws(string log)
    {
        var regex = DrawRegex();
        var draws = log.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var drawLog in draws)
        {
            var draw = new ColorTriplet(0, 0, 0);
            foreach (var match in regex.Matches(drawLog).Cast<Match>())
            {
                var amount = int.Parse(match.Groups["amount"].Value);
                var color = match.Groups["color"].Value;
                draw = color switch
                {
                    "red" => draw with { Red = draw.Red + amount },
                    "green" => draw with { Green = draw.Green + amount },
                    "blue" => draw with { Blue = draw.Blue + amount },
                    _ => throw new InvalidDataException("Unknown color " + color)
                };
                yield return draw;
            }
        }
    }

    public record ColorTriplet(int Red, int Green, int Blue);
}
