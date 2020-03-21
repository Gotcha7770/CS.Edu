using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    [Flags]
    public enum SplitOptions
    {
        None = 0x0,
        /// <summary>
        /// Если задан этот флаг, граничное значение включается и в предыдущую
        /// и в последующую подпоследовательность
        /// </summary>
        /// <example>
        /// (x, y, z) => x < y ? y < z : y > z
        /// [1, 2, 3, 2, 1] -> [1, 2, 3], [3, 2, 1]
        /// </example>
        IncludeBorders = 0x1
    }

    public static class EnumerableExt
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
        {
            return source.Except(EnumerableEx.Return(item));
        }

        public static IEnumerable<T> If<T>(this IEnumerable<T> thenSource, Func<bool> condition, IEnumerable<T> elseSource)
        {
            return EnumerableEx.If(condition, thenSource, elseSource);
        }

        public static IEnumerable<T> FlatZip<T>(this IEnumerable<T> left, IEnumerable<T> right)
        {
            using (var leftEnumerator = left.GetEnumerator())
            using (var rightEnumerator = right.GetEnumerator())
            {
                while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
                {
                    yield return leftEnumerator.Current;
                    yield return rightEnumerator.Current;
                }
            }
        }

        public static IEnumerable<T> OfType<T>(this IEnumerable<T> source)
        {
            var genericType = new GenericType(typeof(T));
            return source.Where(x => x.IsSubclassOf(genericType));
        }

        public static IEnumerable OfType(this IEnumerable source, GenericType constraint)
        {
            foreach (var item in source)
            {
                if (item.IsSubclassOf(constraint))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Relation<T> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return SplitIterator(source, relation);
        }

        static IEnumerable<IEnumerable<T>> SplitIterator<T>(IEnumerable<T> source, Relation<T> relation, SplitOptions options = SplitOptions.None)
        {
            List<T> acc;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                acc = new List<T> { enumerator.Current };

                while (enumerator.MoveNext())
                {
                    T item = enumerator.Current;
                    if (relation(acc.Last(), item))
                    {
                        acc.Add(item);
                    }
                    else
                    {
                        yield return acc;
                        acc = new List<T> { item };
                    }
                }
            }

            if (acc.Count > 0)
                yield return acc;
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source,
                                                           Relation<T, T, T> relation,
                                                           SplitOptions options = SplitOptions.None)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            if (options.HasFlag(SplitOptions.IncludeBorders))
                return SplitWithBordersIterator(source, relation);
            else
                return SplitIterator(source, relation);
        }

        static IEnumerable<IEnumerable<T>> SplitIterator<T>(IEnumerable<T> source, Relation<T, T, T> relation)
        {
            List<T> acc;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T first = enumerator.Current;

                if (!enumerator.MoveNext())
                {
                    yield return EnumerableEx.Return(first);
                    yield break;
                }

                T second = enumerator.Current;
                acc = new List<T> { first, second };

                while (enumerator.MoveNext())
                {
                    T item = enumerator.Current;
                    if (relation(first, second, item))
                    {
                        acc.Add(item);
                    }
                    else
                    {
                        yield return acc;
                        
                        acc = new List<T> { item };
                    }

                    first = second;
                    second = item;
                }
            }

            if (acc.Count > 0)
                yield return acc;
        }

        static IEnumerable<IEnumerable<T>> SplitWithBordersIterator<T>(IEnumerable<T> source,
                                                                       Relation<T, T, T> relation)
        {
            List<T> acc;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T first = enumerator.Current;

                if (!enumerator.MoveNext())
                {
                    yield return EnumerableEx.Return(first);
                    yield break;
                }

                T second = enumerator.Current;
                acc = new List<T> { first, second };

                while (enumerator.MoveNext())
                {
                    T item = enumerator.Current;
                    if (relation(first, second, item))
                    {
                        acc.Add(item);
                    }
                    else
                    {
                        yield return acc;
                        
                        acc = new List<T> { second, item };
                    }

                    first = second;
                    second = item;
                }
            }

            if (acc.Count > 0)
                yield return acc;
        }

        public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, Relation<T> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return TakeWhileIterator(source, relation);
        }

        public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, Relation<T, T, T> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return TakeWhileIterator(source, relation);
        }

        static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source, Relation<T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T prev = enumerator.Current;
                yield return prev;

                while (enumerator.MoveNext())
                {
                    if (relation(prev, enumerator.Current))
                    {
                        prev = enumerator.Current;
                        yield return prev;
                    }
                    else
                        yield break;
                }
            }
        }

        static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source, Relation<T, T, T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T first = enumerator.Current;
                yield return first;

                if (!enumerator.MoveNext())
                    yield break;

                T second = enumerator.Current;
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

        public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> source, Relation<T> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return SkipWhileIterator(source, relation);
        }

        static IEnumerable<T> SkipWhileIterator<T>(IEnumerable<T> source, Relation<T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T prev = enumerator.Current;
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
        /// Постранично разбивает входную последовательность 
        ///в соответствии с заданным параметром размера страницы
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> source, int pageSize)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize));

            return PaginateIterator(source, pageSize);
        }

        static IEnumerable<IEnumerable<T>> PaginateIterator<T>(IEnumerable<T> source, int pageSize)
        {
            IEnumerable<T> left = source;
            while (left.Any())
            {
                yield return left.Take(pageSize);
                left = left.Skip(pageSize);
            }
        }
    }
}
