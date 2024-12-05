using Utilities.Extensions;
using UpdateRule = (int first, int then);

namespace Year2024;

public class Day5
{
    [Theory]
    [FileLines("data_sample.txt", 143)]
    [FileLines("data.txt", 4924)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var chunks = data.ChunkBy("").ToArray();
        var rules = chunks[0]
            .Select(l => l.Split('|'))
            .Select(g => (first: int.Parse(g[0]), then: int.Parse(g[1])))
            .ToArray();

        var sum = chunks[1]
            .Select(l => l
                .Split(',')
                .Select(int.Parse)
                .ToArray()
            )
            .Where(u => IsValidUpdate(u, rules))
            .Select(u => u[(int)Math.Floor(u.Length / 2.0)])
            .Sum();

        Assert.Equal(result, sum);
    }

    private static bool IsValidUpdate(int[] update, UpdateRule[] rules)
    {
        var sentinels = rules
            .Where(t => update.Contains(t.first))
            .Select(kvp => kvp.then)
            .ToList();

        foreach (var item in update)
        {
            if (sentinels.Contains(item))
            {
                return false;
            }

            var unlocked = rules.Where(t => t.first == item);
            foreach (var (_, u) in unlocked)
            {
                _ = sentinels.Remove(u);
            }
        }

        return true;
    }

    [Theory]
    [FileLines("data_sample.txt", 123)]
    [FileLines("data.txt", 6085)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var chunks = data.ChunkBy("");
        var rules = chunks
            .First()
            .Select(l => l.Split('|'))
            .Select(g => (first: int.Parse(g[0]), then: int.Parse(g[1])))
            .ToArray();

        var sum = chunks
            .Skip(1)
            .First()
            .Select(l => l
                .Split(',')
                .Select(int.Parse)
                .ToArray()
            )
            .Where(u => !IsValidUpdate(u, rules))
            .Select(u => FixUpdateOrder(u, rules))
            .Select(u => u[(int)Math.Floor(u.Length / 2.0)])
            .Sum();

        Assert.Equal(result, sum);
    }

    private static int[] FixUpdateOrder(int[] update, UpdateRule[] rules)
    {
        rules = rules
            .Where(t => update.Contains(t.first) && update.Contains(t.then))
            .ToArray();

        var order = new int[update.Length];
        var og = new List<int>(update);

        for (var i = 0; i < order.Length; i++)
        {
            var next = og.First(o => !rules.Any(t => t.then == o));
            _ = og.Remove(next);
            order[i] = next;
            rules = rules.Where(t => t.first != next).ToArray();
        }

        return order;
    }
}
