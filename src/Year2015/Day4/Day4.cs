using System.Security.Cryptography;
using System.Text;

namespace Year2015;

public class Day4
{
    [Theory]
    [InlineData("abcdef", 609043)]
    [InlineData("pqrstuv", 1048970)]
    [InlineData("yzbqklnj", 282749)]
    public void Part1(string data, int result)
    {
        var number = Enumerable.Range(0, int.MaxValue)
            .First(i =>
            {
                var buffer = Encoding.UTF8.GetBytes($"{data}{i}");
                var hash = Convert.ToHexString(MD5.HashData(buffer));
                return hash.StartsWith("00000");
            });

        Assert.Equal(result, number);
    }

    [Theory]
    [InlineData("yzbqklnj", 9962624)]
    public void Part2(string data, int result)
    {
        var number = Enumerable.Range(0, int.MaxValue)
            .First(i =>
            {
                var buffer = Encoding.UTF8.GetBytes($"{data}{i}");
                var hash = MD5.HashData(buffer);
                return hash[0] == 0 && hash[1] == 0 && hash[2] == 0;
            });

        Assert.Equal(result, number);
    }
}
