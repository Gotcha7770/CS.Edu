using System;

namespace CS.Edu.Core.Extensions
{
    public static class Predicates
    {
        public static Predicate<T> True<T>() => TruePredicate<T>.Value;

        public static Predicate<T> False<T>() => FalsePredicate<T>.Value;

        private static class TruePredicate<T>
        {
            internal static readonly Predicate<T> Value = AlwaysTrue;

            private static bool AlwaysTrue(T value) => true;
        }

        private static class FalsePredicate<T>
        {
            internal static readonly Predicate<T> Value = AlwaysFalse;

            private static bool AlwaysFalse(T value) => false;
        }
    }
}