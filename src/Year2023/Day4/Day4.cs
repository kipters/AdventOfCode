using System.ComponentModel;
using Utilities.Extensions;
using Xunit.Abstractions;

namespace Year2023;

public class Day4
{
    [Theory]
    [FileLines("data_sample.txt", 13)]
    [FileLines("data.txt", 27454)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var points = data
            .Select(ParseCard)
            .Select(t => t.drawn.Intersect(t.winning))
            .Select(d => d.Count())
            .Where(n => n > 0)
            .Select(p => Math.Pow(2, p - 1))
            .Sum()
            ;

        Assert.Equal(result, points);
    }

    [Theory]
    [FileLines("data_sample.txt", 30)]
    [FileLines("data.txt", 6857330)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var cards = data
            .Select(ParseCard)
            .Select(c => (win: c.drawn.Intersect(c.winning).Count(), qty: 1))
            .ToArray();

        for (var i = 0; i < cards.Length; i++)
        {
            var (win, qty) = cards[i];

            if (win == 0)
            {
                continue;
            }

            for (var x = 0; x < win; x++)
            {
                cards[i + x + 1].qty += qty;
            }
        }

        var totalCards = cards.Sum(c => c.qty);

        Assert.Equal(result, totalCards);
    }

    private static (int[] winning, int[] drawn) ParseCard(string line)
    {
        var colonIndex = line.IndexOf(':') + 1;
        var pipeIndex = line.IndexOf('|');
        var afterPipeIndex = pipeIndex + 1;

        var winning = line[colonIndex..pipeIndex]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();

        var drawn = line[afterPipeIndex..]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToArray();

        return (winning, drawn);
    }
}
