using System.Buffers;
using Utilities.Extensions;
using Xunit.Abstractions;

namespace Year2015;

public class Day5(ITestOutputHelper helper)
{
    [Theory]
    [InlineData(new[] { "ugknbfddgicrmopn" }, 1)]
    [InlineData(new[] { "aaa" }, 1)]
    [InlineData(new[] { "jchzalrnumimnmhp" }, 0)]
    [InlineData(new[] { "haegwjzuvuyypxyu" }, 0)]
    [InlineData(new[] { "dvszwmarrgswjxmb" }, 0)]
    [FileLines("data.txt", 258)]
    public void Part1(IEnumerable<string> data, int result)
    {
        var sv = SearchValues.Create("aeiou".AsSpan());
        var nice = data.Count(l => IsNice(l, sv));

        Assert.Equal(result, nice);

        static bool IsNice(string line, SearchValues<char> sv)
        {
            var vowels = 0;
            var repeats = false;
            var last = '§';

            for (var i = 0; i < line.Length; i++)
            {
                var c = line[i];

                vowels += sv.Contains(c) ? 1 : 0;
                repeats |= last == c;

                var hasForbidden = (last, c) switch
                {
                    ('a', 'b') => true,
                    ('c', 'd') => true,
                    ('p', 'q') => true,
                    ('x', 'y') => true,
                    _ => false
                };

                last = c;

                if (hasForbidden)
                {
                    return false;
                }
            }

            return repeats && vowels >= 3;
        }
    }

    [Theory]
    [InlineData(new[] { "qjhvhtzxzqqjkmpb" }, 1)]
    [InlineData(new[] { "xxyxx" }, 1)]
    [InlineData(new[] { "uurcxstgmygtbstg" }, 0)]
    [InlineData(new[] { "ieodomkazucvgmuy" }, 0)]
    [InlineData(new[] { "aaaba" }, 0)]
    [FileLines("data.txt", 53)]
    public void Part2(IEnumerable<string> data, int result)
    {
        var nice = data.Count(IsNice);
        Assert.Equal(result, nice);

        bool IsNice(string line)
        {
            var triplet = line
                .SlidingWindow(3)
                .Any(t => t[0] == t[2]);

            var digram = line
                .SlidingWindow(2)
                .Select(w => new string(w))
                .SkipBy((a, b) => a == b)
                .GroupBy(_ => _)
                .Any(g => g.Count() >= 2)
                // .ToArray()
                ;
            helper.WriteLine($"{triplet} {digram} {triplet && digram}");
            return triplet && digram;
        }
    }
}
