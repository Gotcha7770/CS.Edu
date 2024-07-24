using System;
using System.Linq;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<TResult> FullOuterJoin<TA, TB, TKey, TResult>(
        this IEnumerable<TA> left,
        IEnumerable<TB> right,
        Func<TA, TKey> leftKeySelector,
        Func<TB, TKey> rightKeySelector,
        Func<TA, TB, TKey, TResult> projection,
        TA leftDefault = default,
        TB rightDefault = default,
        IEqualityComparer<TKey> comparer = null)
    {
        comparer ??= EqualityComparer<TKey>.Default;
        var leftLookup = left.ToLookup(leftKeySelector, comparer);
        var rightLookup = right.ToLookup(rightKeySelector, comparer);

        var keys = new HashSet<TKey>(leftLookup.Select(p => p.Key), comparer);
        keys.UnionWith(rightLookup.Select(p => p.Key));

        var join = from key in keys
            from xleft in leftLookup[key].DefaultIfEmpty(leftDefault)
            from xright in rightLookup[key].DefaultIfEmpty(rightDefault)
            select projection(xleft, xright, key);

        return join;
    }
}