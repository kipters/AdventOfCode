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

    public static T Product<T>(this IEnumerable<T> sequence)
        where T : IMultiplyOperators<T, T, T>, IMultiplicativeIdentity<T, T>
    {
        var a = T.MultiplicativeIdentity;
        using var enumerator = sequence.GetEnumerator();

        while (enumerator.MoveNext())
        {
            a *= enumerator.Current;
        }

        return a;
    }
}
