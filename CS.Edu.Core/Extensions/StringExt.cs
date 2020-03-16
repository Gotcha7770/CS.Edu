using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class StringExt
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsNumber(this string value)
        {
            return value.All(c => char.IsDigit(c));
        }
    }
}