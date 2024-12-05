namespace Utilities.Extensions;

public static class ObjectExtensions
{
    public static TOutput Then<TInput, TOutput>(this TInput self, Func<TInput, TOutput> transform)
    {
        ArgumentNullException.ThrowIfNull(transform);
        return transform(self);
    }
}
