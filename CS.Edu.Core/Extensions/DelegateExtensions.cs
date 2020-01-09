using System;

namespace CS.Edu.Core.Extensions
{
    public static class DelegateExtensions
    {

        public static Predicate<T> True<T>() => TruePredicate<T>.Value;

        public static Predicate<T> False<T>() => FalsePredicate<T>.Value;

        public static Func<T, T> Identity<T>() => IdentityFunc<T>.Value;

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

        private static class IdentityFunc<T>
        {
            internal static readonly Func<T, T> Value = Id;

            private static T Id(T value) => value;
        }
    }
}
