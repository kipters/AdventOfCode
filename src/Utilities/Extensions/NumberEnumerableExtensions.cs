using System.Numerics;

namespace Utilities.Extensions;

public static class NumberEnumerableExtensions
{
    public static IEnumerable<T> ProgressiveSum<T>(this IEnumerable<T> sequence)
        where T : IAdditionOperators<T, T, T>, IAdditiveIdentity<T, T>
    {
        ArgumentNullException.ThrowIfNull(sequence);
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
        ArgumentNullException.ThrowIfNull(sequence);
        var a = T.MultiplicativeIdentity;
        using var enumerator = sequence.GetEnumerator();

        while (enumerator.MoveNext())
        {
            a *= enumerator.Current;
        }

        return a;
    }

    public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
    {
        while (b != T.Zero)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static (T min, T max) Bounds<T>(this IEnumerable<T> sequence)
        where T : IComparisonOperators<T, T, bool>
    {
        ArgumentNullException.ThrowIfNull(sequence);
        using var enumerator = sequence.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException("The collection is empty");
        }

        var min = enumerator.Current!;
        var max = enumerator.Current!;

        while (enumerator.MoveNext())
        {
            var val = enumerator.Current;
            min = val < min ? val : min;
            max = val > max ? val : max;
        }

        return (min, max);
    }

    public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
        => a / GreatestCommonDivisor(a, b) * b;

    public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
        => values.Aggregate(LeastCommonMultiple);

    public static IEnumerable<T> CountTo<T>(this T start, T end)
        where T : IComparisonOperators<T, T, bool>, IIncrementOperators<T>
    {
        for (var i = start; i <= end; i++)
        {
            yield return i;
        }
    }
}
