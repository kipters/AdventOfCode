using System.Globalization;

namespace Utilities.Extensions;

public static class StringExtensions
{
    public static int ToInteger(this string self) => self.ToParsable<int>();
    public static T ToParsable<T>(this string self) where T : IParsable<T> => T.Parse(self, CultureInfo.InvariantCulture);
}
