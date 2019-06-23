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

        static IEnumerable<IEnumerable<TSource>> TunedPaginateIterator<TSource>(IEnumerable<TSource> source, int pageSize)
        {
            IEnumerable<TSource> InnerIterator(IEnumerator<TSource> e, int c)
            {
                //bool guard = true;
                int count = 0;

                while (count < c && e.MoveNext())
                {                    
                    yield return e.Current;
                    count++;

                    //if (count++ < c)
                    //    break;

                    //guard = e.MoveNext();
                }

                ///guard = count > 0 && count <= c;
            }

            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    yield return InnerIterator(e, pageSize);
                }
            };
        }
    }
}
