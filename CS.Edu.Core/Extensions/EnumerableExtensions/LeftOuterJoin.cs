using System;
using System.Linq;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<TResult> LeftOuterJoin<TLeft, TRight, TKey, TResult>(
        this IEnumerable<TLeft> outer,
        IEnumerable<TRight> inner,
        Func<TLeft, TKey> outerKeySelector,
        Func<TRight, TKey> innerKeySelector,
        Func<TLeft, TRight, TResult> resultSelector)
    {
        return from left in outer
               join right in inner on outerKeySelector(left) equals innerKeySelector(right) into temp
               from right in temp.DefaultIfEmpty()
               select resultSelector(left, right);
    }
}