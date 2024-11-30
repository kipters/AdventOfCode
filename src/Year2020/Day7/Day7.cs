using System.Text.RegularExpressions;
using Ruleset = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, int>>;

namespace Year2020;

public partial class Day7
{
    [Theory]
    [FileLines("data_sample.txt", 4)]
    [FileLines("data.txt", 332)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var rules = ParseRules(data);

        var total = rules.Keys
            .Count(k => LeadsTo(rules, k, "shiny gold"));

        Assert.Equal(result, total);
    }

    private static bool LeadsTo(Ruleset ruleset, string start, string target)
    {
        return ruleset.TryGetValue(start, out var rules) &&
            (rules.ContainsKey(target) ||
            rules.Keys.Any(r => LeadsTo(ruleset, r, target)));
    }

    private static Ruleset ParseRules(IEnumerable<string> data)
    {
        var splitter = LineSplitterRegex();

        return data
            .Select(l => splitter.Match(l))
            .ToDictionary(m => m.Groups[1].Value, m => ParseContent(m.Groups[2].Value));
    }

    private static Dictionary<string, int> ParseContent(string content) => content == "no other bags."
        ? []
        : ContentRegex()
        .Matches(content)
        .ToDictionary(
            m => m.Groups[2].Value,
            m => int.Parse(m.Groups[1].Value)
        );

    [Theory]
    [FileLines("data_sample.txt", 32)]
    [FileLines("data_sample2.txt", 126)]
    [FileLines("data.txt", 10875)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var rules = ParseRules(data);

        var content = GetContentQuantity(rules, "shiny gold");

        Assert.Equal(result, content);
    }

    private static int GetContentQuantity(Ruleset rules, string key) => rules.TryGetValue(key, out var content)
        ? content.Sum(p => p.Value) + content.Sum(p => p.Value * GetContentQuantity(rules, p.Key))
        : 0;

    [GeneratedRegex(@"(\w+ \w+) bags contain (.*)")]
    private static partial Regex LineSplitterRegex();

    [GeneratedRegex(@"(\d+) (\w+ \w+)")]
    private static partial Regex ContentRegex();
}
