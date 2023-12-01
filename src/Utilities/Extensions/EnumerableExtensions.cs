namespace Utilities.Extensions;

public static class EnumerableExtensions
{
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
