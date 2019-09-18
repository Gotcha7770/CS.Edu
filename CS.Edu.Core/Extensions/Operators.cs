using System;

namespace CS.Edu.Core.Extensions
{
    public static class Operators
    {
        public static T Max<T>(T first, T second) where T : IComparable
        {
            return first.CompareTo(second) > -1 ? first : second;
        }

        public static T Max<T>(T first, T second, T third) where T : IComparable
        {
            return Max(first.CompareTo(second) > -1 ? first : second, third);
        }

        public static T Min<T>(T first, T second) where T : IComparable
        {
            return first.CompareTo(second) < 1 ? first : second;
        }

        public static T Min<T>(T first, T second, T third) where T : IComparable
        {
            return Min(first.CompareTo(second) < 1 ? first : second, third);
        }
    }
}