using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Iterators;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static class JoinedEnumerable
{
    public static JoinedEnumerable<TElement> Inner<TElement>(this IEnumerable<TElement> source)
    {
        return Wrap(source, false);
    }

    public static JoinedEnumerable<TElement> Outer<TElement>(this IEnumerable<TElement> source)
    {
        return Wrap(source, true);
    }

    public static JoinedEnumerable<TElement> Wrap<TElement>(IEnumerable<TElement> source, bool isOuter)
    {
        JoinedEnumerable<TElement> joinedSource = source as JoinedEnumerable<TElement>
                                                  ?? new JoinedEnumerable<TElement>(source, isOuter);
        return joinedSource;
    }

    public static IEnumerable<TResult> Join<TOuter, TInner, TKey, TResult>(
        this JoinedEnumerable<TOuter> outer,
        IEnumerable<TInner> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector,
        IEqualityComparer<TKey> comparer = null)
    {
        ArgumentNullException.ThrowIfNull(outer);
        ArgumentNullException.ThrowIfNull(inner);
        ArgumentNullException.ThrowIfNull(outerKeySelector);
        ArgumentNullException.ThrowIfNull(innerKeySelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        bool isLeftOuter = outer.IsOuter;
        bool isRightOuter = inner is JoinedEnumerable<TInner> { IsOuter: true };

        return (isLeftOuter, isRightOuter) switch
        {
            (true, true) => FullOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer),
            (true, _) => LeftOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer),
            (_, true) => RightOuterJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer),
            _ => Enumerable.Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector, comparer)
        };
    }

    public static IEnumerable<TResult> LeftOuterJoin<TOuter, TInner, TKey, TResult>(
        this IEnumerable<TOuter> outer,
        IEnumerable<TInner> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector,
        IEqualityComparer<TKey> comparer = null)
    {
        var innerLookup = inner.ToLookup(innerKeySelector, comparer);

        foreach (var outerItem in outer)
        foreach (var innerItem in innerLookup[outerKeySelector(outerItem)].DefaultIfEmpty())
            yield return resultSelector(outerItem, innerItem);
    }

    public static IEnumerable<TResult> RightOuterJoin<TOuter, TInner, TKey, TResult>(
        this IEnumerable<TOuter> outer,
        IEnumerable<TInner> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector,
        IEqualityComparer<TKey> comparer = null)
    {
        var outerLookup = outer.ToLookup(outerKeySelector, comparer);

        foreach (var innerItem in inner)
        foreach (var outerItem in outerLookup[innerKeySelector(innerItem)].DefaultIfEmpty())
            yield return resultSelector(outerItem, innerItem);
    }

    public static IEnumerable<TResult> FullOuterJoin<TOuter, TInner, TKey, TResult>(
        this IEnumerable<TOuter> outer,
        IEnumerable<TInner> inner,
        Func<TOuter, TKey> outerKeySelector,
        Func<TInner, TKey> innerKeySelector,
        Func<TOuter, TInner, TResult> resultSelector,
        IEqualityComparer<TKey> comparer = null)
    {
        var outerLookup = outer.ToLookup(outerKeySelector, comparer);
        var innerLookup = inner.ToLookup(innerKeySelector, comparer);

        foreach (var innerGrouping in innerLookup)
            if (!outerLookup.Contains(innerGrouping.Key))
                foreach (TInner innerItem in innerGrouping)
                    yield return resultSelector(default, innerItem);

        foreach (var outerGrouping in outerLookup)
        foreach (var innerItem in innerLookup[outerGrouping.Key].DefaultIfEmpty())
        foreach (var outerItem in outerGrouping)
            yield return resultSelector(outerItem, innerItem);
    }
}