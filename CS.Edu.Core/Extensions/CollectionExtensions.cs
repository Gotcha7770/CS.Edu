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