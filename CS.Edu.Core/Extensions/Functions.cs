using System;
using System.Collections.Concurrent;

namespace CS.Edu.Core.Extensions;

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

    /// <summary>
    /// <see cref="https://stackoverflow.com/questions/1254995/thread-safe-memoization"/>
    /// </summary>
    public static Func<TIn, TOut> Memoize<TIn, TOut>(this Func<TIn, TOut> function)
    {
        var cache = new ConcurrentDictionary<TIn, Lazy<TOut>>();
        return x => cache.GetOrAdd(x, new Lazy<TOut>(() => function(x))).Value;
    }

    public static Func<T1, T2, TOut> Memoize<T1, T2, TOut>(this Func<T1, T2, TOut> function)
    {
        var cache = new ConcurrentDictionary<(T1, T2), Lazy<TOut>>();
        return (x, y) => cache.GetOrAdd((x, y), new Lazy<TOut>(() => function(x, y))).Value;
    }

    public static Func<T, T> Identity<T>() => IdentityFunc<T>.Value;

    private static class IdentityFunc<T>
    {
        internal static readonly Func<T, T> Value = Id;

        private static T Id(T value) => value;
    }
}