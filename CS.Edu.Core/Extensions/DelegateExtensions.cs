using System;

namespace CS.Edu.Core.Extensions
{
    public static class DelegateExtensions
    {
        public static Func<T, T, bool> ToFunc<T>(this Relation<T> relation)
        {
            return new Func<T, T, bool>((x, y) => relation(x,y));
        }

        public static Func<T1, T2, bool> ToFunc<T1, T2>(this Relation<T1, T2> relation)
        {
            return new Func<T1, T2, bool>((x, y) => relation(x,y));
        }

        public static Func<T, T, T> ToFunc<T>(this Merge<T> mergeFunc)
        {
            return new Func<T, T, T>((x,y) => mergeFunc(x, y));
        } 

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
