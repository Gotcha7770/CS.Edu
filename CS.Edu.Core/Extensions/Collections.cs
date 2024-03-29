using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace CS.Edu.Core.Extensions;

public static class Collections
{
    public static void Invalidate<TKey, TValue>(this IList<TValue> source,
        IEnumerable<TValue> patch,
        Func<TValue, TValue, TValue> mergeFunc,
        Func<TValue, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(patch);
        ArgumentNullException.ThrowIfNull(mergeFunc);
        ArgumentNullException.ThrowIfNull(keySelector);

        Dictionary<TKey, TValue> dic = patch.ToDictionary(keySelector);

        for (int i = source.Count - 1; i >= 0; i--)
        {
            TValue item = source[i];
            TKey key = keySelector(item);

            if (dic.TryGetValue(key, out TValue value))
            {
                source[i] = mergeFunc(item, value);
                dic.Remove(key);
            }
            else
            {
                source.RemoveAt(i);
            }
        }

        foreach (var item in dic.Values)
        {
            source.Add(item);
        }
    }

    public static void AddOrUpdate<T>(this IList<T> source, T item, Func<T, T, T> mergeFunc)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(mergeFunc);

        var index = source.IndexOf(item);
        if (index == -1)
        {
            source.Add(item);
        }
        else
        {
            source[index] = mergeFunc(source[index], item);
        }
    }

    public static IEnumerable<TValue> Merge<TKey, TValue>(this IEnumerable<TValue> source,
        IEnumerable<TValue> patch,
        Func<TValue, TValue, TValue> mergeFunc,
        Func<TValue, TKey> keySelector)
    {
        return source.ToDictionary(keySelector)
            .Merge(patch, mergeFunc, keySelector);
    }

    public static IEnumerable<TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source,
        IEnumerable<TValue> patch,
        Func<TValue, TValue, TValue> mergeFunc,
        Func<TValue, TKey> keySelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(patch);
        ArgumentNullException.ThrowIfNull(mergeFunc);
        ArgumentNullException.ThrowIfNull(keySelector);

        return patch.Select(x => source.TryGetValue(keySelector(x), out TValue value) ? mergeFunc(value, x) : x);
    }

    public static ReadOnlySpan<T> ToSpan<T>(this IReadOnlyCollection<T> source)
    {
        return source switch
        {
            List<T> list => CollectionsMarshal.AsSpan(list),
            T[] array => array.AsSpan(),
            _ => Span<T>.Empty
        };
    }

    public static bool SequenceEqual<T>(this IReadOnlyCollection<T> source, ReadOnlySpan<T> span)
    {
        return source.ToSpan()
            .SequenceEqual(span);
    }

    public static bool SequenceEqual<T>(this IReadOnlyCollection<T> source, ReadOnlyMemory<T> memory)
    {
        return source.SequenceEqual(memory.Span);
    }
}