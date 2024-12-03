namespace Year2021;

public class Day2
{
    [Theory]
    [FileLines("data_sample.txt", 150)]
    [FileLines("data.txt", 1762050)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var depth = data
            .Select(l => l.Split(' '))
            .Select(g => (cmd: g[0], arg: int.Parse(g[1])))
            .Aggregate((depth: 0, position: 0), (a, t) => t switch
            {
                ("forward", int v) => a with { position = a.position + v },
                ("down", int v) => a with { depth = a.depth + v },
                ("up", int v) => a with { depth = a.depth - v },
                _ => throw new ArgumentException($"Invalid command {t.cmd}")
            }, a => a.depth * a.position);

        Assert.Equal(result, depth);
    }

    [Theory]
    [FileLines("data_sample.txt", 900)]
    [FileLines("data.txt", 1855892637)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var depth = data
            .Select(l => l.Split(' '))
            .Select(g => (cmd: g[0], arg: int.Parse(g[1])))
            .Aggregate((depth: 0, position: 0, aim: 0), (a, t) => t switch
            {
                ("forward", int v) => a with
                {
                    position = a.position + v,
                    depth = a.depth + v * a.aim
                },
                ("down", int v) => a with { aim = a.aim + v },
                ("up", int v) => a with { aim = a.aim - v },
                _ => throw new ArgumentException($"Invalid command {t.cmd}")
            }, a => a.depth * a.position);

        Assert.Equal(result, depth);
    }
}
