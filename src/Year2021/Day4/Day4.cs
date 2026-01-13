using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Utilities.Extensions;

namespace Year2021;

public class Day4
{
    [Theory]
    [FileLines("data_sample.txt", 4512)]
    [FileLines("data.txt", 11774)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var drawSequence = ParseDrawSequence(data);
        var boards = ParseBoards(data);

        var score = drawSequence
            .StatefulSelect((boards, drawn: -1), (n, bs) =>
            {
                foreach (var board in boards)
                {
                    board.Draw(n);
                }

                return (boards, n);
            })
            .First(t => t.boards.Any(b => b.IsWinning))
            .Then(t => t.boards.First(b => b.IsWinning).UnmarkedNumbers().Sum() * t.drawn);

        Assert.Equal(result, score);
    }

    [Theory]
    [FileLines("data_sample.txt", 1924)]
    [FileLines("data.txt", 4495)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var drawSequence = ParseDrawSequence(data);
        var boards = ParseBoards(data);

        var score = drawSequence
            .StatefulSelect((boards, drawn: -1), (n, bs) =>
            {
                foreach (var board in boards)
                {
                    board.Draw(n);
                }

                return (boards, n);
            })
            .Aggregate(new List<(int drawn, Board board)>(), (acc, bs) =>
            {
                var winner = bs.boards.Where(b => b.IsWinning).ToArray();

                if (winner.Length == 0)
                {
                    return acc;
                }

                foreach (var w in winner)
                {
                    acc.Add((bs.drawn, w));
                    _ = bs.boards.Remove(w);
                }

                return acc;
            })
            .Last()
            .Then(t => t.drawn * t.board.UnmarkedNumbers().Sum());
    }

    private static List<Board> ParseBoards(IEnumerable<string> data) =>
        [.. data
            .Skip(2)
            .ChunkBy("")
            .Select(Board.Parse)];

    private static int[] ParseDrawSequence(IEnumerable<string> data) =>
        [.. data
            .First()
            .Split(',')
            .Select(int.Parse)];

    internal class Board
    {
        public int[][] Rows { get; private set; } = [];
        public int[][] Columns { get; private set; } = [];
        public static Board Parse(string[] lines)
        {
            var rows = lines
                .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                .ToArray();

            var columns = Enumerable.Range(0, rows[0].Length)
                .Select(i => rows.Select(r => r[i]).ToArray())
                .ToArray();

            return new Board { Rows = rows, Columns = columns };
        }

        public void Draw(int number)
        {
            Rows = [.. Rows.Select(r => r.Where(n => n != number).ToArray())];
            Columns = [.. Columns.Select(c => c.Where(n => n != number).ToArray())];
            IsWinning = Rows.Any(r => r.Length == 0) || Columns.Any(c => c.Length == 0);
        }

        public bool IsWinning { get; private set; }

        public IEnumerable<int> UnmarkedNumbers() => Rows.SelectMany(_ => _);
    }
}
