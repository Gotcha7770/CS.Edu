using System;
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

    public Group<TKey, T> this[TKey key] => Match(
        l => l.GetValueOrDefault(key, Empty),
        _ => Empty);
}

public static class Groups
{
    public static TResult SelectMany<TKey, T, TCollection, TResult>(
        this Group<TKey, T> source,
        Func<Group<TKey, T>, Group<TKey, TCollection>> groupSelector,
        Func<Group<TKey, T>, Group<TKey, TCollection>, TResult> resultSelector)
    {
        var intermediate = groupSelector(source);

        return resultSelector(source, intermediate);
    }

    public static TResult SelectMany<TInput, TKey, T, TResult>(
        this TInput source,
        Func<TInput, Group<TKey, T>> groupSelector,
        Func<TInput, Group<TKey, T>, TResult> resultSelector)

    {
        var intermediate = groupSelector(source);

        return resultSelector(source, intermediate);
    }
}