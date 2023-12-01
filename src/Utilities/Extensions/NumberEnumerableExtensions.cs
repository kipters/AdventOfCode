using System.Numerics;

namespace Utilities.Extensions;

public static class NumberEnumerableExtensions
{
    public static IEnumerable<T> ProgressiveSum<T>(this IEnumerable<T> sequence)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        var accumulator = T.AdditiveIdentity;

        foreach (var number in sequence)
        {
            accumulator += number;
            yield return accumulator;
        }
    }
}
