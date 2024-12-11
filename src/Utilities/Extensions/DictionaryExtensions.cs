using System.Numerics;

namespace Utilities.Extensions;

public static class DictionaryExtensions
{
    public static void SetOrIncrement<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        where TValue : IAdditionOperators<TValue, TValue, TValue>
    {
        ArgumentNullException.ThrowIfNull(dict);

        dict[key] = dict.TryGetValue(key, out var current) ? current + value : value;
    }
}
