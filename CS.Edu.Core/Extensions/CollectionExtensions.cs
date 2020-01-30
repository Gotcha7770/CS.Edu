using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class CollectionExt
    {
        public static void Invalidate<T>(this IList<T> list,
                                         ICollection<T> newItems,
                                         Merge<T> mergeFunc,
                                         IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;

            Dictionary<int, T> missed = list.ToDictionary(x => comparer.GetHashCode(x));

            foreach (var item in newItems)
            {
                list.AddOrUpdate(item, mergeFunc, comparer);
                var key = comparer.GetHashCode(item);
                missed.Remove(key);
            }

            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                var key = comparer.GetHashCode(item);
                if (missed.ContainsKey(key))
                {
                    list.RemoveAt(i);
                }
            }
        }

        public static void Invalidate2<TKey, TValue>(this IList<TValue> source,
                                                     ICollection<TValue> update,
                                                     Merge<TValue> mergeFunc,
                                                     Func<TValue, TKey> keySelector)
        {
            Dictionary<TKey, TValue> patch = update.ToDictionary(keySelector);

            for (int i = source.Count - 1; i >= 0; i--)
            {
                TValue item = source[i];
                TKey key = keySelector(item);

                if (patch.TryGetValue(key, out TValue value))
                {
                    source[i] = mergeFunc(item, value);
                    patch.Remove(key);
                }
                else
                {
                    source.RemoveAt(i);
                }
            }

            foreach (var item in patch.Values)
            {
                source.Add(item);
            }
        }

        public static IEnumerable<TValue> Merge<TKey, TValue>(this ICollection<TValue> source,
                                                              ICollection<TValue> patch,
                                                              Merge<TValue> mergeFunc,
                                                              Func<TValue, TKey> keySelector)
        {
            Func<TValue, IEnumerable<TValue>, TValue> resultSelector = (x, y) =>
            {
                return y switch
                {
                    _ when y.IsNullOrEmpty() => x,
                    _ => mergeFunc(y.First(), x)
                };
            };

            return patch.GroupJoin(source, keySelector, keySelector, resultSelector);
        }

        public static IEnumerable<TValue> Merge2<TKey, TValue>(this ICollection<TValue> source,
                                                              ICollection<TValue> patch,
                                                              Merge<TValue> mergeFunc,
                                                              Func<TValue, TKey> keySelector)
        {
            var dic = source.ToDictionary(x => keySelector(x));

            foreach (var p in patch)
            {
                TKey key = keySelector(p);
                if(dic.TryGetValue(key, out var value))
                {
                    yield return mergeFunc(value, p);
                }
                else
                {
                    yield return p;
                }
            }
        }

        public static void AddOrUpdate<T>(this IList<T> list,
                                          T item,
                                          Merge<T> mergeFunc,
                                          IEqualityComparer<T> comparer = null)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (list.IsReadOnly)
                throw new InvalidOperationException("list must be writeable!");

            if (mergeFunc == null)
                throw new ArgumentNullException(nameof(mergeFunc));

            comparer = comparer ?? EqualityComparer<T>.Default;

            int index = list.IndexOf(item, comparer);
            if (index >= 0)
            {
                T updated = mergeFunc(list[index], item);
                list[index] = updated;
            }
            else
            {
                list.Add(item);
            }
        }

        public static int IndexOf<T>(this IList<T> list, T item, IEqualityComparer<T> comparer)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (comparer.Equals(list[i], item))
                    return i;
            }

            return -1;
        }
    }
}