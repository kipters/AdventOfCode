using System.Diagnostics.CodeAnalysis;

namespace Utilities.Comparers;

public sealed class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    where T : IEquatable<T>
{
    public bool Equals(T[]? x, T[]? y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Length != y.Length)
        {
            return false;
        }

        for (var i = 0; i < x.Length; i++)
        {
            if (!x[i].Equals(y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode([DisallowNull] T[] obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return obj.GetHashCode();
    }
}
