using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class EnumerableExt
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Постранично разбивает входную последовательность в соответствии с заданным параметром размера страницы
        /// </summary>
        public static IEnumerable<IEnumerable<TSource>> Paginate<TSource>(this IEnumerable<TSource> source, int pageSize)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (pageSize < 1) throw new ArgumentOutOfRangeException(nameof(pageSize));

            return PaginateIterator(source, pageSize);
        }

        static IEnumerable<IEnumerable<TSource>> PaginateIterator<TSource>(IEnumerable<TSource> source, int pageSize)
        {
            IEnumerable<TSource> left = source;
            while (left.Any())
            {
                yield return left.Take(pageSize);
                left = left.Skip(pageSize);
            }
        }

        public static IEnumerable<TSource> TakeLastLinkedList<TSource>(this IEnumerable<TSource> source, int count)
        {
            var linkedList = new LinkedList<TSource>();

            foreach (var item in source)
            {
                linkedList.AddLast(item);

                if(linkedList.Count > count)
                    linkedList.RemoveFirst();
            }

            return linkedList;
        }

        public static IEnumerable<TSource> TakeLastReverse<TSource>(this IEnumerable<TSource> source, int count)
        {
            return source.Reverse().Take(count).Reverse();
        }
    }
}
