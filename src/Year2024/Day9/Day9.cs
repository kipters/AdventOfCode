using Utilities.Extensions;

namespace Year2024;

public class Day9
{
    [Theory]
    [FileData("data_sample.txt", 1928)]
    [FileData("data.txt", 6337921897505)]
    public void Part1(string data, long result)
    {
        var nodes = ParseNodes(data);

        var checksum = ReadDefragmented(nodes)
            .Select((id, i) => id * i)
            .Sum();

        Assert.Equal(result, checksum);
    }

    private static INode[] ParseNodes(string data)
    {
        if (data.Length % 2 != 0)
        {
            data += "0";
        }

        var nodes = data
            .Select(c => (int)char.GetNumericValue(c))
            .Chunk(2)
            .SelectMany<int[], INode>((c, i) => [new File(i, c[0]), new FreeSpace(c[1])])
            .ToArray();
        return nodes;
    }

    private static IEnumerable<long> ReadFromEnd(INode[] nodes) => nodes
        .Reverse()
        .Where(n => n is File f && !f.Consumed)
        .Cast<File>()
        .Inspect(f => f.Consumed = true)
        .SelectMany(f => f.Id.Repeat(f.Length));

    private static IEnumerable<long> ReadDefragmented(INode[] nodes)
    {
        using var backEnumerator = ReadFromEnd(nodes).GetEnumerator();

        foreach (var n in nodes)
        {
            if (n is File { Consumed: true })
            {
                break;
            }
            else if (n is File file)
            {
                file.Consumed = true;
                for (var i = 0; i < file.Length; i++)
                {
                    yield return file.Id;
                }
            }
            else if (n is FreeSpace free)
            {
                for (var i = 0; i < free.Length; i++)
                {
                    yield return backEnumerator.MoveNext()
                        ? backEnumerator.Current
                        : throw new Exception("No more appendable items!");
                }
            }
        }

        while (backEnumerator.MoveNext())
        {
            yield return backEnumerator.Current;
        }
    }

    [Theory]
    [FileData("data_sample.txt", 2858)]
    [FileData("data.txt", 6362722604045)]
    public void Part2(string data, long result)
    {
        var nodes = ParseNodes(data);

        var defrag = ReadDefragmentedFiles(nodes)
            .SelectMany(n => n switch
            {
                File f => f.Id.Repeat(n.Length),
                FreeSpace s => 0L.Repeat(s.Length),
                _ => throw new NotImplementedException()
            })
            .ToList();

        var checksum = defrag
            .Select((b, i) => b * i)
            .Sum();


        Assert.Equal(result, checksum);
    }

    private static List<INode> ReadDefragmentedFiles(INode[] nodes)
    {
        var l = new List<INode>(nodes);
        while (true)
        {
            if (l.LastOrDefault(n => n is File { Consumed: false }) is not File lastNotConsumed)
            {
                break;
            }

            lastNotConsumed.Consumed = true;
            var indexOfBlock = l.IndexOf(lastNotConsumed);

            var firstBigEnoughSpace = l.FirstOrDefault(n => n is FreeSpace fs && fs.Length >= lastNotConsumed.Length);

            if (firstBigEnoughSpace is null)
            {
                continue;
            }

            var indexOfFreeSpace = l.IndexOf(firstBigEnoughSpace);

            if (indexOfFreeSpace >= indexOfBlock)
            {
                continue;
            }

            l.Insert(indexOfBlock, new FreeSpace(lastNotConsumed.Length));
            _ = l.Remove(lastNotConsumed);

            var padding = firstBigEnoughSpace.Length - lastNotConsumed.Length;

            if (padding > 0)
            {
                l.Insert(indexOfFreeSpace, new FreeSpace(padding));
            }

            l.Insert(indexOfFreeSpace, lastNotConsumed);
            _ = l.Remove(firstBigEnoughSpace);
        }

        return l;
    }

    private interface INode
    {
        public int Length { get; }
    }

    private record File(long Id, int Length) : INode
    {
        public bool Consumed { get; set; } = false;
    }

    private record FreeSpace(int Length) : INode;
}
