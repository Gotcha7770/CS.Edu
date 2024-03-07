﻿using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IAsyncEnumerable<TResult> Join<TLeft, TRight, TKey, TResult>(
        this IEnumerable<TLeft> outer,
        IAsyncEnumerable<TRight> inner,
        Func<TLeft, TKey> outerKeySelector,
        Func<TRight, TKey> innerKeySelector,
        Func<TLeft, TRight, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(outer);
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(outerKeySelector);
        ArgumentNullException.ThrowIfNull(innerKeySelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return JoinIterator(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
    }

    private static async IAsyncEnumerable<TResult> JoinIterator<TLeft, TRight, TKey, TResult>(
        IEnumerable<TLeft> outer,
        IAsyncEnumerable<TRight> inner,
        Func<TLeft, TKey> outerKeySelector,
        Func<TRight, TKey> innerKeySelector,
        Func<TLeft, TRight, TResult> resultSelector)
    {
        var lookup = outer.ToLookup(outerKeySelector);

        await foreach(var right in inner)
        {
            var key = innerKeySelector(right);

            foreach(var left in lookup[key])
            {
                yield return resultSelector(left, right);
            }
        }
    }

    public static IAsyncEnumerable<TResult> Join<TLeft, TRight, TKey, TResult>(
        this IAsyncEnumerable<TLeft> outer,
        IEnumerable<TRight> inner,
        Func<TLeft, TKey> outerKeySelector,
        Func<TRight, TKey> innerKeySelector,
        Func<TLeft, TRight, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(outer);
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(outerKeySelector);
        ArgumentNullException.ThrowIfNull(innerKeySelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return JoinIterator(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
    }

    private static async IAsyncEnumerable<TResult> JoinIterator<TLeft, TRight, TKey, TResult>(
        IAsyncEnumerable<TLeft> outer,
        IEnumerable<TRight> inner,
        Func<TLeft, TKey> outerKeySelector,
        Func<TRight, TKey> innerKeySelector,
        Func<TLeft, TRight, TResult> resultSelector)
    {
        var lookup = await outer.ToLookupAsync(outerKeySelector);

        foreach(var right in inner)
        {
            var key = innerKeySelector(right);

            foreach(var left in lookup[key])
            {
                yield return resultSelector(left, right);
            }
        }
    }
}