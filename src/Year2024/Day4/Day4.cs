#pragma warning disable IDE0046 // Convert to conditional expression
namespace Year2024;

public class Day4
{
    [Theory]
    [FileLines("data_sample.txt", 18)]
    [FileLines("data.txt", 2504)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var arr = data.ToArray();
        var rows = arr.Length;
        var columns = arr[0].Length;

        var lut = new Dictionary<uint, char>();
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                lut.Add(CoordsToKey(row, col), arr[row][col]);
            }
        }

        var count = 0;
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                var center = Read(lut, row, col);

                if (center != 'X')
                {
                    continue;
                }

                count += Check(lut, row, col,  0, -1);
                count += Check(lut, row, col, +1, -1);
                count += Check(lut, row, col, +1,  0);
                count += Check(lut, row, col, +1, +1);

                count += Check(lut, row, col,  0, +1);
                count += Check(lut, row, col, -1, +1);
                count += Check(lut, row, col, -1,  0);
                count += Check(lut, row, col, -1, -1);
            }
        }

        Assert.Equal(result, count);
    }

    private static uint CoordsToKey(int row, int col) => (uint)(row << 16) | (uint)col;
    private static char Read(Dictionary<uint, char> lut, int row, int col) => lut.GetValueOrDefault(CoordsToKey(row, col), '.');
    private static int Check(Dictionary<uint, char> lut, int row, int col, int xDir, int yDir)
    {
        char[] seq = [
            Read(lut, row + 1 * yDir, col + 1 * xDir),
            Read(lut, row + 2 * yDir, col + 2 * xDir),
            Read(lut, row + 3 * yDir, col + 3 * xDir)
        ];

        return seq switch {
            ['M', 'A', 'S'] => 1,
            _ => 0
        };
    }

    [Theory]
    [FileLines("data_sample.txt", 9)]
    [FileLines("data.txt", 1923)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var arr = data.ToArray();
        var rows = arr.Length;
        var columns = arr[0].Length;

        var lut = new Dictionary<uint, char>();
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                lut.Add(CoordsToKey(row, col), arr[row][col]);
            }
        }

        var count = 0;
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < columns; col++)
            {
                var center = Read(lut, row, col);

                if (center != 'A')
                {
                    continue;
                }

                char[] adj = [
                    Read(lut, row - 1, col - 1),
                    Read(lut, row + 1, col + 1),

                    Read(lut, row - 1, col + 1),
                    Read(lut, row + 1, col - 1),
                ];

                count += adj switch
                {
                    ['M', 'S', 'M', 'S'] => 1,
                    ['M', 'S', 'S', 'M'] => 1,
                    ['S', 'M', 'S', 'M'] => 1,
                    ['S', 'M', 'M', 'S'] => 1,
                    _ => 0
                };
            }
        }

        Assert.Equal(result, count);
    }
}
