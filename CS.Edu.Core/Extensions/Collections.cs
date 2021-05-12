using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class Collections
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
            foreach (TValue p in patch)
            {
                yield return source.TryGetValue(keySelector(p), out TValue value)
                    ? mergeFunc(value, p)
                    : p;
            }
        }

        public static void Swap<T>(ref T first, ref T second)
        {
            T tmp = first;
            first = second;
            second = tmp;
        }

        public static void Swap<T>(this IList<T> list, int first, int second)
        {
            T tmp = list[first];
            list[first] = list[second];
            list[second] = tmp;
        }

        public static void PartialSort<T>(T[] input, IComparer<T> comparer, Predicate<T> predicate)
        {
            int count = input.Length;

            switch (count)
            {
                case 0:
                case 1:
                    break;
                default:
                    Sort(input, comparer, predicate);
                    break;
            }
        }

        private static void Sort<T>(T[] input, IComparer<T> comparer, Predicate<T> predicate)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!predicate(input[i]))
                    continue;

                for (int j = i; j < input.Length; j++)
                {
                    if (!predicate(input[j]))
                        continue;

                    if (comparer.Compare(input[i], input[j]) > 0)
                        Swap(ref input[i], ref input[j]);
                }
            }
        }

        public static T[] Copy<T>(this T[] source)
        {
            T[] result = new T[source.Length];
            Array.Copy(source, result, source.Length);

            return result;
        }
    }
}