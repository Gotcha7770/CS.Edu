using System;
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
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));
            if (thenSource == null)
                throw new ArgumentNullException(nameof(thenSource));
            if (elseSource == null)
                throw new ArgumentNullException(nameof(elseSource));

            return EnumerableEx.Defer(() => condition() ? thenSource : elseSource);
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Relation<T> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            return SplitIterator(source, relation);
        }

        static IEnumerable<IEnumerable<T>> SplitIterator<T>(IEnumerable<T> source, Relation<T> relation)
        {
            int countToSkip = 0;
            while (source.Skip(countToSkip).Any())
            {
                yield return TakeWhileIterator(source, countToSkip, relation);

                countToSkip += CounterIterator(source, countToSkip, relation);
            }
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
            int countToSkip = 0;
            while (source.Skip(countToSkip).Any())
            {
                yield return TakeWhileIterator(source, countToSkip, relation);

                countToSkip += CounterIterator(source, countToSkip, relation);
            }
        }

        static IEnumerable<IEnumerable<T>> SplitWithBordersIterator<T>(IEnumerable<T> source,
                                                                       Relation<T, T, T> relation)
        {
            int countToSkip = 0;
            while (source.Skip(countToSkip).Any())
            {
                yield return TakeWhileIterator(source, countToSkip, relation);

                countToSkip += CounterWithBorderIterator(source, countToSkip, relation);
            }
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

        static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source,
                                                   int countToSkip,
                                                   Relation<T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext())
                    countToSkip--;

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

        static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source,
                                                   int countToSkip,
                                                   Relation<T, T, T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext())
                    countToSkip--;

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
                        break;
                }
            }
        }

        static int CounterIterator<T>(IEnumerable<T> source, int countToSkip, Relation<T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext())
                    countToSkip--;

                if (!enumerator.MoveNext())
                    return 0;

                int result = 1;
                T prev = enumerator.Current;

                while (enumerator.MoveNext() && relation(prev, enumerator.Current))
                {
                    result++;
                    prev = enumerator.Current;
                }

                return result;
            }
        }

        static int CounterIterator<T>(IEnumerable<T> source, int countToSkip, Relation<T, T, T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext())
                    countToSkip--;

                if (!enumerator.MoveNext())
                    return 0;

                T first = enumerator.Current;

                if (!enumerator.MoveNext())
                    return 1;

                int result = 2;
                T second = enumerator.Current;

                while (enumerator.MoveNext() && relation(first, second, enumerator.Current))
                {
                    result++;
                    first = second;
                    second = enumerator.Current;
                }

                return result;
            }
        }

        static int CounterWithBorderIterator<T>(IEnumerable<T> source,
                                                int countToSkip,
                                                Relation<T, T, T> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext())
                    countToSkip--;

                if (!enumerator.MoveNext())
                    return 0;

                T first = enumerator.Current;

                if (!enumerator.MoveNext())
                    return 1;

                int result = 2;
                T second = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    if (relation(first, second, enumerator.Current))
                    {
                        result++;
                        first = second;
                        second = enumerator.Current;
                    }
                    else
                    {
                        result--;
                        break;
                    }
                }

                return result;
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
