using System.Linq;

namespace CS.Edu.Core.Extensions;

public static class Strings
{
    public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

    public static bool IsNullOrWhiteSpace(this string value) => string.IsNullOrWhiteSpace(value);

    public static bool IsNumber(this string value) => value.All(char.IsDigit);

    public static bool IsUpper(this string value) => value.All(char.IsUpper);
}