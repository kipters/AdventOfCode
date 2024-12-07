using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Year2024;

public partial class Day7
{
    [Theory]
    [FileLines("data_sample.txt", 3749)]
    [FileLines("data.txt", 2664460013123)]
    public void Part1(IEnumerable<string> data, long result)
    {
        var regex = NumberRegex();
        var validCalibrations = data
            .Select(l => regex
                .Matches(l)
                .Select(m => m.Value.ToParsable<long>())
                .ToArray())
            .Select(r =>
            {
                var result = r[0];
                var combos = (1 << (r.Length - 2)) - 1;

                var canBeFixed =
                    IsValid(r[1], 2, r, '+', result) ||
                    IsValid(r[1], 2, r, '*', result);

                return canBeFixed ? result : 0;
            })
            .Sum();

        Assert.Equal(result, validCalibrations);
    }

    private static bool IsValid(long operand, int index, long[] values, char op, long maxResult)
    {
        if (index == values.Length)
        {
            return operand == maxResult;
        }

        var next = values[index];
        var myResult = op switch
        {
            '+' => operand + next,
            '*' => operand * next,
            _ => throw new InvalidOperationException("Not allowed"),
        };

        return myResult <= maxResult
            && (IsValid(myResult, index + 1, values, '+', maxResult)
            || IsValid(myResult, index + 1, values, '*', maxResult));
    }

    [Theory]
    [FileLines("data_sample.txt", 11387)]
    [FileLines("data.txt", 426214131924213)]
    public void Part2(IEnumerable<string> data, long result)
    {
        var regex = NumberRegex();
        var validCalibrations = data
            .Select(l => regex
                .Matches(l)
                .Select(m => m.Value.ToParsable<long>())
                .ToArray())
            .Select(r =>
            {
                var result = r[0];
                var combos = (1 << (r.Length - 2) * 2) - 1;

                var canBeFixed =
                    IsValid2(r[1], 2, r, '+', result) ||
                    IsValid2(r[1], 2, r, '*', result) ||
                    IsValid2(r[1], 2, r, '|', result);

                return canBeFixed ? result : 0;
            })
            .Sum();

        Assert.Equal(result, validCalibrations);
    }

    private static bool IsValid2(long operand, int index, long[] values, char op, long maxResult)
    {
        if (index == values.Length)
        {
            return operand == maxResult;
        }

        var next = values[index];
        var myResult = op switch
        {
            '+' => operand + next,
            '*' => operand * next,
            '|' => long.Parse($"{operand}{next}"),
            _ => throw new InvalidOperationException("Not allowed"),
        };

        return myResult <= maxResult
            && (IsValid2(myResult, index + 1, values, '+', maxResult)
            || IsValid2(myResult, index + 1, values, '*', maxResult)
            || IsValid2(myResult, index + 1, values, '|', maxResult));
    }

    [GeneratedRegex(@"(\d)+")]
    private static partial Regex NumberRegex();
}
