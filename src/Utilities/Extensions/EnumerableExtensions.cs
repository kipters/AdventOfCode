using System.Numerics;

namespace Utilities.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Range<T>(T start, long count)
        where T : IIncrementOperators<T>
    {
        var initial = start;
        for (var i = 0; i < count; i++)
        {
            yield return initial;
            initial++;
        }
    }

    public static IEnumerable<T> SkipBy<T>(this IEnumerable<T> sequence, Func<T, T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(predicate);
        using var enumerator = sequence.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            yield break;
        }

        var previous = enumerator.Current;
        yield return previous;

        while (enumerator.MoveNext())
        {
            var shouldSkip = predicate(previous, enumerator.Current);
            previous = enumerator.Current;
            if (!shouldSkip)
            {
                yield return enumerator.Current;
            }
        }
    }

    public static string Stringify<T>(this IEnumerable<T> sequence) => string.Join(' ', sequence);
    public static string AsString(this Span<char> sequence) => new(sequence);

    public static IEnumerable<T> Tap<T>(this IEnumerable<T> sequence, Action<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(predicate);
        using var enumerator = sequence.GetEnumerator();
        while (enumerator.MoveNext())
        {
            predicate(enumerator.Current);
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<TResult> Mutate<TMutable, TInput, TResult>(this IEnumerable<TInput> sequence
        , TMutable mutable
        , Action<TMutable, TInput> function
        , Func<TMutable, TResult> predicate)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(predicate);
        ArgumentNullException.ThrowIfNull(function);
        foreach (var item in sequence)
        {
            function(mutable, item);
            yield return predicate(mutable);
        }
    }

    public static IEnumerable<TState> StatefulSelect<TInput, TState>(this IEnumerable<TInput> sequence
        , TState initialState
        , Func<TInput, TState, TState> mutator)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(mutator);
        using var enumerator = sequence.GetEnumerator();
        var state = initialState;

        while (enumerator.MoveNext())
        {
            state = mutator(enumerator.Current, state);
            yield return state;
        }
    }

    public static IEnumerable<T> Loop<T>(this IEnumerable<T> sequence)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        while (true)
        {
            using var enumerator = sequence.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> sequence, int items)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        using var fillEnumerator = sequence.GetEnumerator();
        while (fillEnumerator.MoveNext())
        {
            yield return fillEnumerator.Current;
        }

        using var enumerator = sequence.GetEnumerator();
        for (var i = 0; i < items && enumerator.MoveNext(); i++)
        {
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<T[]> SlidingWindow<T>(this IEnumerable<T> sequence, int windowSize)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        var buffer = new List<T>(windowSize);
        using var enumerator = sequence.GetEnumerator();

        for (var i = 0; i < windowSize; i++)
        {
            if (enumerator.MoveNext())
            {
                buffer.Add(enumerator.Current);
            }
            else
            {
                yield return buffer.ToArray();
                yield break;
            }
        }

        yield return buffer.ToArray();

        while (enumerator.MoveNext())
        {
            buffer.RemoveAt(0);
            buffer.Add(enumerator.Current);
            yield return buffer.ToArray();
        }
    }

    public static IEnumerable<T[]> ChunkBy<T>(this IEnumerable<T> sequence, T separator) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(sequence);
        var buffer = new List<T>();

        foreach (var item in sequence)
        {
            if (item.Equals(separator))
            {
                if (buffer.Count != 0)
                {
                    yield return buffer.ToArray();
                    buffer.Clear();
                }
            }
            else
            {
                buffer.Add(item);
            }
        }

        if (buffer.Count != 0)
        {
            yield return buffer.ToArray();
        }
    }

    public static IDictionary<T, int> ToFrequencies<T>(this IEnumerable<T> sequence) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(sequence);
        var dict = new Dictionary<T, int>();

        foreach (var item in sequence)
        {
            var count = dict.GetValueOrDefault(item, 0) + 1;
            dict[item] = count;
        }

        return dict;
    }

    public static IEnumerable<TSource> TransformWhile<TSource>(this TSource source
        , Func<TSource, TSource> func
        , Func<TSource, bool> shouldContinue)
    {
        ArgumentNullException.ThrowIfNull(func);
        ArgumentNullException.ThrowIfNull(shouldContinue);

        var result = source;
        do
        {
            result = func(result);
            yield return result;
        } while (shouldContinue(result));
    }

    public static IEnumerable<(TFirst a, TSecond b)> CartesianProduct<TFirst, TSecond>(this IEnumerable<TFirst> a, IEnumerable<TSecond> b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);
        using var outerEnumerator = a.GetEnumerator();

        while (outerEnumerator.MoveNext())
        {
            var x = outerEnumerator.Current;
            var innerEnumerator = b.GetEnumerator();

            while (innerEnumerator.MoveNext())
            {
                var y = innerEnumerator.Current;
                yield return (x, y);
            }
        }
    }

    public static IEnumerable<(T a, T b)> CartesianProduct<T>(this IEnumerable<T> sequence) => sequence.CartesianProduct(sequence);

    public static IEnumerable<T> Repeat<T>(this T item, int count)
    {
        for (var i = 0; i < count; i++)
        {
            yield return item;
        }
    }

    public static IEnumerable<T> Assert<T>(this IEnumerable<T> sequence, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(sequence);
        ArgumentNullException.ThrowIfNull(predicate);

        using var enumerator = sequence.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var current = enumerator.Current;
            yield return predicate(current)
                ? current
                : throw new AssertionException($"Assertion failed at item {current}");
        }
    }
}
