using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Monads;

public class Group<TKey, T> : Either<IReadOnlyDictionary<TKey, Group<TKey, T>>, T[]>
{
    public static Group<TKey, T> Empty { get; } = new(default);

    public Group(TKey key, IEnumerable<Group<TKey, T>> children) : base(children.ToDictionary(x => x.Key))
    {
        Key = key;
    }

    public Group(TKey key, IEnumerable<T> values) : this(key, values.ToArray()) { }


    public Group(TKey key, params T[] values) : base(values)
    {
        Key = key;
    }

    public TKey Key { get; }

    // public Optional<Group<TKey, T>> this[TKey key] => Match(
    //     l => l[key],
    //     _ => Optional.None<Group<TKey, T>>());

    public Group<TKey, T> this[TKey key] => Match(
        l => l[key],
        _ => new Group<TKey, T>(key));
}

public static class Groups
{
    public static Group<TKey, TResult> Select<TKey, T, TResult>(
        this Group<TKey, T> source,
        Func<Group<TKey, T>, Group<TKey, TResult>> selector)
    {
        return selector(source);
    }

    public static Group<TKey, TResult> SelectMany<TKey, T, TCollection, TResult>(
        this Group<TKey, T> source,
        Func<Group<TKey, T>, Group<TKey, TCollection>> collectionSelector,
        Func<Group<TKey, T>, Group<TKey, TCollection>, Group<TKey, TResult>> resultSelector)
    {
        var intermediate = collectionSelector(source);

        return resultSelector(source, intermediate);
    }
}