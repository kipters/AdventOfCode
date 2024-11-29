using System.Security.Cryptography;
using System.Text;
using Utilities.Extensions;
using Xunit.Abstractions;

namespace Year2016;

public class Day5
{
    [Theory]
    [InlineData("abc", "18f47a30")]
    [InlineData("uqwqemis", "1a3099aa")]
    public void Part1(string doorId, string expectedPassword)
    {
        var password = Enumerable.Range(0, 100_000_000)
            .Select(i => MD5.HashData(Encoding.UTF8.GetBytes($"{doorId}{i}")))
            .Where(h => h[0] == 0 && h[1] == 0 && h[2] < 0x10)
            .Select(h => h[2].ToString("x1"))
            .Take(8)
            .Aggregate("", (s, p) => s + p)
            ;

        Assert.Equal(expectedPassword, password);
    }

    [Theory]
    [InlineData("abc", "05ace8e3")]
    [InlineData("uqwqemis", "694190cd")]
    public void Part2(string doorId, string expectedPassword)
    {
        var password = Enumerable.Range(0, 100_000_000)
            .Select(i => MD5.HashData(Encoding.UTF8.GetBytes($"{doorId}{i}")))
            .Where(h => h[0] == 0 && h[1] == 0 && h[2] < 0x10)
            .Select(h => (position: h[2], part: h[3].ToString("x2")[0]))
            .Where(t => t.position < 8)
            .DistinctBy(t => t.position)
            .Take(8)
            .Aggregate(new char[8], (s, t) =>
            {
                s[t.position] = t.part;
                return s;
            }, s => new string(s));

        Assert.Equal(expectedPassword, password);
    }
}
