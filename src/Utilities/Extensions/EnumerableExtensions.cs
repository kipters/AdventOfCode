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

    public static IEnumerable<T> Inspect<T>(this IEnumerable<T> sequence, Action<T> predicate)
    {
        using var enumerator = sequence.GetEnumerator();
        while (enumerator.MoveNext())
        {
            predicate(enumerator.Current);
            yield return enumerator.Current;
        }
    }

    public static IEnumerable<TResult> Mutate<TMutant, TInput, TResult>(this IEnumerable<TInput> sequence
        , TMutant mutant
        , Action<TMutant, TInput> function
        , Func<TMutant, TResult> predicate)
    {
        foreach (var item in sequence)
        {
            function(mutant, item);
            yield return predicate(mutant);
        }
    }

    public static IEnumerable<T> Loop<T>(this IEnumerable<T> sequence)
    {
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
}
