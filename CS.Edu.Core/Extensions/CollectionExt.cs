using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class CollectionExt
    {
        public static void Invalidate<TKey, TValue>(this IList<TValue> source,
                                                    IEnumerable<TValue> patch,
                                                    Merge<TValue> mergeFunc,
                                                    Func<TValue, TKey> keySelector)
        {
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

        public static IEnumerable<TValue> Merge<TKey, TValue>(this IEnumerable<TValue> source,
                                                              IEnumerable<TValue> patch,
                                                              Merge<TValue> mergeFunc,
                                                              Func<TValue, TKey> keySelector)
        {
            return source.ToDictionary(keySelector)
                .Merge(patch, mergeFunc, keySelector);
        }

        public static IEnumerable<TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source,
                                                              IEnumerable<TValue> patch,
                                                              Merge<TValue> mergeFunc,
                                                              Func<TValue, TKey> keySelector)
        {
            foreach (var p in patch)
            {
                yield return source.TryGetValue(keySelector(p), out var value)
                    ? mergeFunc(value, p)
                    : p;
            }
        }
    }
}