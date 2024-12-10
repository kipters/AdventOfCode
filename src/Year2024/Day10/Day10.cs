namespace Year2024;

public class Day10
{
    [Theory]
    [FileLines("data_sample.txt", 2)]
    [FileLines("data_sample2.txt", 4)]
    [FileLines("data.txt", 557)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var map = data
            .Select(l => l.ToCharArray().Select(c => c == '.' ? -1 : (int) char.GetNumericValue(c)).ToArray())
            .ToArray();

        var maxX = map[0].Length;
        var maxY = map.Length;

        var score = 0;
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                var position = map[y][x];
                if (position != 0)
                {
                    continue;
                }

                var headScore = GetReachableDestinations(map, x, y, new HashSet<(int x, int y)>());
                score += headScore;
            }
        }

        Assert.Equal(result, score);
    }

    private static int GetReachableDestinations(int[][] map, int x, int y, HashSet<(int x, int y)> found)
    {
        var val = map[y][x];

        if (val == 9)
        {
            var isNew = found.Add((x, y));
            return isNew ? 1 : 0;
        }

        (int x, int y)[] adjacent = [(x, y - 1), (x + 1, y), (x, y + 1), (x - 1, y)];

        var childScores = 0;
        foreach (var (ax, ay) in adjacent)
        {
            if (ax < 0 || ay < 0)
            {
                continue;
            }

            if (ax >= map[0].Length || ay >= map.Length)
            {
                continue;
            }

            var next = map[ay][ax];

            if (next != val + 1)
            {
                continue;
            }

            var score = GetReachableDestinations(map, ax, ay, found);
            childScores += score;
        }

        return childScores;
    }

    [Theory]
    [FileLines("data_sample3.txt", 81)]
    [FileLines("data_sample4.txt", 3)]
    [FileLines("data_sample5.txt", 13)]
    [FileLines("data_sample6.txt", 227)]
    [FileLines("data.txt", 1062)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var map = data
            .Select(l => l.ToCharArray().Select(c => c == '.' ? -1 : (int) char.GetNumericValue(c)).ToArray())
            .ToArray();

        var maxX = map[0].Length;
        var maxY = map.Length;

        var score = 0;
        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                var position = map[y][x];
                if (position != 0)
                {
                    continue;
                }

                var headScore = GetTrailRating(map, x, y);
                score += headScore;
            }
        }

        Assert.Equal(result, score);
    }

    private static int GetTrailRating(int[][] map, int x, int y)
    {
        var val = map[y][x];

        if (val == 9)
        {
            return 1;
        }

        (int x, int y)[] adjacent = [(x, y - 1), (x + 1, y), (x, y + 1), (x - 1, y)];

        var childScores = 0;
        foreach (var (ax, ay) in adjacent)
        {
            if (ax < 0 || ay < 0)
            {
                continue;
            }

            if (ax >= map[0].Length || ay >= map.Length)
            {
                continue;
            }

            var next = map[ay][ax];

            if (next != val + 1)
            {
                continue;
            }

            var score = GetTrailRating(map, ax, ay);
            childScores += score;
        }

        return childScores;
    }
}
