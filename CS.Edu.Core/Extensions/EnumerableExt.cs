using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public delegate bool Relation<T>(T first, T second);

    public delegate bool Relation<in T1, in T2>(T1 first, T2 second);

    public delegate bool Relation<in T1, in T2, in T3>(T1 first, T2 second, T3 third);

    public static class EnumerableExt
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, Relation<TSource> relation)
        {
            while (source.Any())
            {
                yield return source.TakeWhile(relation);
                source = source.SkipWhile(relation);
            }
        }

        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Relation<TSource> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return TakeWhileIterator(source, relation);
        }

        public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Relation<TSource, TSource, TSource> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return TakeWhileIterator(source, relation);
        }

        static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Relation<TSource> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                TSource prev = enumerator.Current;
                yield return prev;

                while (enumerator.MoveNext())
                {
                    if (!relation(prev, enumerator.Current))
                        break;

                    prev = enumerator.Current;
                    yield return enumerator.Current;
                }
            }
        }

        static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Relation<TSource, TSource, TSource> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                TSource first;
                TSource second;

                if (!enumerator.MoveNext())
                    yield break;

                first = enumerator.Current;
                yield return first;

                if (!enumerator.MoveNext())
                    yield break;

                second = enumerator.Current;
                yield return second;

                while (enumerator.MoveNext())
                {
                    if (relation(first, second, enumerator.Current))
                        yield return enumerator.Current;
                    else
                        yield break;

                    first = second;
                    second = enumerator.Current;
                }
            }
        }

        public static IEnumerable<TSource> SkipWhile<TSource>(this IEnumerable<TSource> source, Relation<TSource> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return SkipWhileIterator(source, relation);
        }

        static IEnumerable<TSource> SkipWhileIterator<TSource>(IEnumerable<TSource> source, Relation<TSource> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                TSource prev = enumerator.Current;
                bool yielding = false;

                while (enumerator.MoveNext())
                {
                    if (!yielding)
                    {
                        if (relation(prev, enumerator.Current))
                        {
                            prev = enumerator.Current;
                        }
                        else
                        {
                            yielding = true;
                            yield return enumerator.Current;
                        }
                    }
                    else
                    {
                        yield return enumerator.Current;
                    }
                }
            }
        }

        /// <summary>
        /// Постранично разбивает входную последовательность в соответствии с заданным параметром размера страницы
        /// </summary>
        public static IEnumerable<IEnumerable<TSource>> Paginate<TSource>(this IEnumerable<TSource> source, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

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

        public static IEnumerable<TSource> TakeLastArray<TSource>(this IEnumerable<TSource> source, int count)
        {
            var array = source.ToArray();
            var dest = new TSource[count];
            Array.Copy(array, array.Length - count, dest, 0, count);

            return dest;
        }

        public static IEnumerable<TSource> TakeLastList<TSource>(this IEnumerable<TSource> source, int count)
        {
            var list = source.ToList();

            return list.GetRange(list.Count - count, count);
        }

        public static IEnumerable<TSource> TakeLastLinkedList<TSource>(this IEnumerable<TSource> source, int count)
        {
            var linkedList = new LinkedList<TSource>();

            foreach (var item in source)
            {
                linkedList.AddLast(item);

                if (linkedList.Count > count)
                    linkedList.RemoveFirst();
            }

            return linkedList;
        }

        public static IEnumerable<TSource> TakeLastReverse<TSource>(this IEnumerable<TSource> source, int count)
        {
            return source.Reverse().Take(count).Reverse();
        }

        public static IEnumerable<TSource> TakeLastSpan<TSource>(this IEnumerable<TSource> source, int count)
        {
            var array = source.ToArray();
            return new Span<TSource>(array, array.Length - count, count).ToArray();
        }
    }
}
