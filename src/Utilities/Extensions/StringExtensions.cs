using System.Globalization;

namespace Utilities.Extensions;

public static class StringExtensions
{
    public static int ToInteger(this string self) => int.Parse(self, CultureInfo.InvariantCulture);
}
