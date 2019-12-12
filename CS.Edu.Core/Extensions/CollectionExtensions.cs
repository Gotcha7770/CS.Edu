using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class CollectionExt
    {
        public static bool TryGetItem<T>(this ICollection<T> source,
                                         Predicate<T> predicate, 
                                         out T result)
        {
            result = source.FirstOrDefault(x => predicate(x));
            return result != null;
        }

        public static void AddOrUpdate<T>(this ICollection<T> source,
                                          T item,
                                          Action<T, T> updateValue,
                                          IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;

            if (source is IList<T> list)
            {
                list.AddOrUpdate(item, updateValue, comparer);
                return;
            }

            T oldItem = source.FirstOrDefault(x => comparer.Equals(x, item));
            if(oldItem != null)
            {            
                updateValue(item, oldItem);
            }
        }

        public static void AddOrUpdate<T>(this IList<T> collection,
                                          T item,
                                          Func<T, T> updateValueFactory,
                                          IEqualityComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = EqualityComparer<T>.Default;
        }
    }
}