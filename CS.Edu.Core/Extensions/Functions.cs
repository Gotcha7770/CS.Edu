using System;

namespace CS.Edu.Core.Extensions
{
    public static class Functions
    {
        public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate)
        {
            return x => predicate(x);
        }

        public static Func<T, T, bool> ToFunc<T>(this Relation<T> relation)
        {
            return (x, y) => relation(x, y);
        }

        public static Func<T1, T2, bool> ToFunc<T1, T2>(this Relation<T1, T2> relation)
        {
            return (x, y) => relation(x, y);
        }

        public static Func<T, T, T> ToFunc<T>(this Merge<T> mergeFunc)
        {
            return (x, y) => mergeFunc(x, y);
        }

        public static Func<T2, TResult> ApplyPartial<T1, T2, TResult>(this Func<T1, T2, TResult> function, T1 arg)
        {
            return x => function(arg, x);
        }

        public static Action<T2> ApplyPartial<T1, T2>(this Action<T1, T2> action, T1 arg)
        {
            return x => action(arg, x);
        }

        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> function)
        {
            return a => b => c => function(a, b, c);
        }

        public static Func<T1, Func<T2, Func<T3, Func<T4, TResult>>>> Curry<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> function)
        {
            return a => b => c => d => function(a, b, c, d);
        }

        public static Func<T, T> Identity<T>() => IdentityFunc<T>.Value;

        private static class IdentityFunc<T>
        {
            internal static readonly Func<T, T> Value = Id;

            private static T Id(T value) => value;
        }
    }
}
