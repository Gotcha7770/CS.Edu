using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

// A[T1] -> A[T2] -> A[T3]...
public interface INestedGrouping<TKey, TSubgroup, T>
{
    //INestedGrouping<TSubKey, T> this[TKey key] { get; }
    //INestedGrouping<L2Key, L3Key, T> GetSubGroup<L3Key>(L1Key l2Key);
    TSubgroup this[TKey key] { get; }
    IEnumerable<T> AsEnumerable();
}

public static class GroupingExtensions
{
    public static IReadOnlyDictionary<L1Key, ILookup<L2Key, T>> ThenBy<L1Key, L2Key, T>(
        this IEnumerable<IGrouping<L1Key, T>> source,
        Func<T, L2Key> subKeySelector)
    {
        return source.ToDictionary(x => x.Key, x => x.ToLookup(subKeySelector));
    }

    public static IReadOnlyDictionary<L1Key, IReadOnlyDictionary<L2Key, ILookup<L3Key, T>>> ThenBy<L1Key, L2Key, L3Key,
        T>(
        this IReadOnlyDictionary<L1Key, ILookup<L2Key, T>> source,
        Func<T, L3Key> subKeySelector)
    {
        var result = new Dictionary<L1Key, IReadOnlyDictionary<L2Key, ILookup<L3Key, T>>>();
        foreach (var kvp in source)
        {
            result[kvp.Key] = kvp.Value.ToDictionary(x => x.Key, x => x.ToLookup(subKeySelector));
        }

        return result;
    }

    public static INestedGrouping<L1Key, L2Key, T> ThenByEx<L1Key, L2Key, T>(
        this IEnumerable<IGrouping<L1Key, T>> source,
        Func<T, L2Key> subKeySelector)
    {
        throw new NotImplementedException();
        //return source.ToDictionary(x => x.Key, x => x.ToLookup(subKeySelector));
    }

    public static INestedGrouping<L1Key, L2Key, T> ThenByEx<L1Key, L2Key, L3Key, T>(
        this INestedGrouping<L1Key, L2Key, T> source,
        Func<T, L3Key> subKeySelector)
    {
        throw new NotImplementedException();
        // var result = new Dictionary<L1Key, IReadOnlyDictionary<L2Key, ILookup<L3Key, T>>>();
        // foreach (var kvp in source)
        // {
        //     result[kvp.Key] = kvp.Value.ToDictionary(x => x.Key, x => x.ToLookup(subKeySelector));
        // }
        //
        // return result;
    }
}