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
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            while (source.Any())
            {
                yield return TakeWhileIterator(source, relation);
                source = SkipWhileIterator(source, relation);
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
                    yield return prev;
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
                    {
                        first = second;
                        second = enumerator.Current;
                        yield return second;
                    }
                    else
                        yield break;
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
    }
}
