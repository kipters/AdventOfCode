using System.Buffers;

namespace Year2015;

public class Day8
{
    [Theory]
    [FileLines("data_sample.txt", 12)]
    [FileLines("data.txt", 1342)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var diff = data
            .Select(DiffLength)
            .Sum();

        Assert.Equal(result, diff);
    }

    private static int DiffLength(string line)
    {
        var chars = line.Length;
        var count = 0;
        var isEscaping = false;

        for (var i = 0; i < chars; i++)
        {
            var c = line[i];

            if (char.IsWhiteSpace(c))
            {
                continue;
            }

            if (!isEscaping && c == '\"')
            {
                continue;
            }

            if (!isEscaping && c == '\\')
            {
                isEscaping = true;
                continue;
            }

            if (isEscaping && c == 'x')
            {
                count++;
                i += 2;
                isEscaping = false;
                continue;
            }

            count++;
            isEscaping = false;
        }

        return chars - count;
    }

    [Theory]
    [FileLines("data_sample.txt", 19)]
    [FileLines("data.txt", 2074)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var diff = data
            .Select(DiffExpansion)
            .Sum();

        Assert.Equal(result, diff);
    }

    private static int DiffExpansion(string line)
    {
        var chars = line.Length;
        var count = 2;

        for (var i = 0; i < chars; i++)
        {
            var c = line[i];

            if (c is '\"' or '\\')
            {
                count++;
            }
        }

        return count;
    }
}
