using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public interface INestedGrouping<TKey, TSubKey, T>
{
    // getter TKey => T
    INestedGrouping<TKey, TSubKey, T> this[TSubKey key] { get; }
}

public static class GroupingExtensions
{
         public static IReadOnlyDictionary<L1Key, ILookup<L2Key, T>> ThenBy<L1Key, L2Key, T>(
         this IEnumerable<IGrouping<L1Key, T>> source,
         Func<T, L2Key> subKeySelector)
     {
         return source.ToDictionary(x => x.Key, x => x.ToLookup(subKeySelector));
     }

     public static IReadOnlyDictionary<L1Key, IReadOnlyDictionary<L2Key, ILookup<L3Key, T>>> ThenBy<L1Key, L2Key, L3Key, T>(
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
}