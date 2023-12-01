using Utilities.Extensions;

namespace Utilities.Tests;

public class NumberEnumerableExtensionsTests
{
    [Theory]
    [InlineData(new[] { 1, 2, 3, 4 }, new[] { 1, 3, 6, 10 })]
    public void ProgressiveSumMatches(int[] sequence, int[] sums)
    {
        var progressive = sequence
            .ProgressiveSum()
            .ToArray();

        Assert.Equal(sums, progressive);
    }
}
