using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Year2020;

public partial class Day4
{
    [Theory]
    [FileLines("data_sample.txt", 2)]
    [FileLines("data.txt", 190)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var valid = data
            .ChunkBy("")
            .Select(ParsePassport)
            .Count(HasNecessaryFields);

        Assert.Equal(result, valid);
    }


    [Theory]
    [FileLines("invalid_data_sample.txt", 0)]
    [FileLines("valid_data_sample.txt", 4)]
    [FileLines("data.txt", 121)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var valid = data
            .ChunkBy("")
            .Select(ParsePassport)
            .Count(IsValid);

        Assert.Equal(result, valid);
    }

    private static IDictionary<string, string> ParsePassport(IEnumerable<string> lines)
    {
        var regex = PairRegex();

        var pairs = lines
            .Select(l => regex.Matches(l) as IEnumerable<Match>)
            .SelectMany(_ => _)
            .Select(m => new KeyValuePair<string, string>(m.Groups[1].Value, m.Groups[2].Value));

        return new Dictionary<string, string>(pairs);
    }

    private static bool HasNecessaryFields(IDictionary<string, string> passport) =>
        passport.ContainsKey("byr") &&
        passport.ContainsKey("iyr") &&
        passport.ContainsKey("eyr") &&
        passport.ContainsKey("hgt") &&
        passport.ContainsKey("hcl") &&
        passport.ContainsKey("ecl") &&
        passport.ContainsKey("pid");

    private static bool IsValid(IDictionary<string, string> passport) =>
        ValidateRange(passport, "byr", 1920, 2002) &&
        ValidateRange(passport, "iyr", 2010, 2020) &&
        ValidateRange(passport, "eyr", 2020, 2030) &&
        ValidateValue(passport, "hgt", IsValidHeight) &&
        ValidateValue(passport, "hcl", ColorRegex().IsMatch) &&
        ValidateValue(passport, "ecl", ValidEyeColors.Contains) &&
        ValidateValue(passport, "pid", l => l.Length == 9 && int.TryParse(l, out var _))
        ;

    private static readonly string[] ValidEyeColors = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];

    private static bool ValidateRange<T>(IDictionary<string, string> passport, string key, T min, T max)
        where T : IParsable<T>, IComparisonOperators<T, T, bool>
    {
        return passport.TryGetValue(key, out var raw) &&
            T.TryParse(raw, CultureInfo.InvariantCulture, out var parsed) &&
            parsed >= min &&
            parsed <= max;
    }

    private static bool ValidateValue(IDictionary<string, string> passport, string key, Func<string, bool> predicate) =>
        passport.TryGetValue(key, out var raw) && predicate(raw);

    private static bool IsValidHeight(string height)
    {
#pragma warning disable IDE0046 // Convert to conditional expression
        if (!height.EndsWith("cm") && !height.EndsWith("in") || !int.TryParse(height[..^2], out var value))
        {
            return false;
        }

        return (height[^2..], value) switch
        {
            ("cm", >= 150 and <= 193) => true,
            ("in", >= 59 and <= 76) => true,
            _ => false
        };
#pragma warning restore IDE0046 // Convert to conditional expression
    }

    [GeneratedRegex(@"(\w{3}):([\w#]+)")]
    private static partial Regex PairRegex();

    [GeneratedRegex(@"#[0-9a-f]{6}")]
    private static partial Regex ColorRegex();
}
