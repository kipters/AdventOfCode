namespace Year2023;

public partial class Day7
{
    [Theory]
    [FileLines("data_sample.txt", 6440)]
    [FileLines("data.txt", 253866470)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var score = SolveGame(data, result,
            a => new Hand(a[0], int.Parse(a[1])));
        Assert.Equal(result, score);
    }

    private static int SolveGame<T>(IEnumerable<string> data, int result, Func<string[], T> handBuilder)
        where T : IBiddable
    {
        var score = data
            .Select(l => l.Split(' '))
            .Select(a => handBuilder(a))
            .Order()
            .Select((h, i) => (i + 1) * h.Bid)
            .Sum();

        return score;
    }

    [Theory]
    [FileLines("data_sample.txt", 5905)]
    [FileLines("data.txt", 254494947)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var score = SolveGame(data, result,
            a => new JokerHand(a[0], int.Parse(a[1])));
        Assert.Equal(result, score);
    }

    private interface IBiddable
    {
        int Bid { get; }
    }

    private enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfKind,
        FullHouse,
        FourOfKind,
        FiveOfKind
    }
}
