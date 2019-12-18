using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Extensions
{
    public static class CollectionExt
    {
        public static void AddOrUpdate<T>(this IList<T> list,
                                          T item,
                                          Merge<T> mergeFunc,
                                          IEqualityComparer<T> comparer = null)
        {
            if (list.IsReadOnly)
                throw new InvalidOperationException();

            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            int index = list.IndexOf(item, comparer);
            if(index >= 0)
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