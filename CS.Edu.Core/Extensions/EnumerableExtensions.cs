using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnumerableEx = System.Linq.EnumerableEx;

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
        /// <code>
        /// (x, y, z) => x &lt; y ? y &lt; z : y &gt; z
        /// </code>
        /// [1, 2, 3, 2, 1] -> [1, 2, 3], [3, 2, 1]
        /// </example>
        IncludeBorders = 0x1
    }

    public static partial class EnumerableExtensions
    {
        public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
        {
            return source.Where(x => !Equals(x, item));
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

        static IEnumerable<IEnumerable<T>> SplitIterator<T>(IEnumerable<T> source, Relation<T> relation)
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

        /// <summary>
        /// Сокращает последовательные вхождения конкретного элемента в последовательности до одного
        /// </summary>
        public static IEnumerable<T> ShrinkDuplicates<T>(this IEnumerable<T> source, T value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ShrinkDuplicatesIterator(source, value);
        }

        static IEnumerable<T> ShrinkDuplicatesIterator<T>(IEnumerable<T> source, T value)
        {
            var comparer = EqualityComparer<T>.Default;
            bool skipping = false;

            foreach (T item in source)
            {
                if (!comparer.Equals(item, value))
                {
                    skipping = false;
                    yield return item;
                }
                else if (!skipping)
                {
                    skipping = true;
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Сокращает последовательные вхождения элементов возвращающих одниковые значения
        /// в последовательности в соответствии с переданной функцией до одного
        /// </summary>
        public static IEnumerable<TValue> ShrinkDuplicates<TKey, TValue>(this IEnumerable<TValue> source,
                                                                         Func<TValue, TKey> keySelector,
                                                                         TKey value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return ShrinkDuplicatesIterator(source, keySelector, value);
        }

        static IEnumerable<TValue> ShrinkDuplicatesIterator<TKey, TValue>(IEnumerable<TValue> source,
                                                                          Func<TValue, TKey> keySelector,
                                                                          TKey value)
        {
            var comparer = EqualityComparer<TKey>.Default;
            bool skipping = false;

            foreach (TValue item in source)
            {
                if (!Equals(keySelector(item), value))
                {
                    skipping = false;
                    yield return item;
                }
                else if (!skipping)
                {
                    skipping = true;
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> ExceptIfLast<T>(this IEnumerable<T> source, T value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ExceptIfLastIterator(source, value);
        }

        static IEnumerable<T> ExceptIfLastIterator<T>(IEnumerable<T> source, T value)
        {
            using(var enumerator = source.GetEnumerator())
            {
                if(!enumerator.MoveNext())
                    yield break;

                var comparer = EqualityComparer<T>.Default;
                T prev = enumerator.Current;

                while(enumerator.MoveNext())
                {
                    yield return prev;

                    prev = enumerator.Current;
                }

                if(!comparer.Equals(prev, value))
                    yield return prev;
            }
        }

        public static IEnumerable<TValue> ExceptIfLast<TValue, TKey>(this IEnumerable<TValue> source,
                                                                     Func<TValue, TKey> keySelector,
                                                                     TKey value)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return ExceptIfLastIterator(source, keySelector, value);
        }

        static IEnumerable<TValue> ExceptIfLastIterator<TValue, TKey>(IEnumerable<TValue> source,
                                                                      Func<TValue, TKey> keySelector,
                                                                      TKey value)
        {
            using(var enumerator = source.GetEnumerator())
            {
                if(!enumerator.MoveNext())
                    yield break;

                var comparer = EqualityComparer<TKey>.Default;
                TValue prev = enumerator.Current;

                while(enumerator.MoveNext())
                {
                    yield return prev;

                    prev = enumerator.Current;
                }

                if(!comparer.Equals(keySelector(prev), value))
                    yield return prev;
            }
        }

        public static IEnumerable<T> Generate<T>(T state, Func<T, T> iterate)
        {
            return Generate(state, Predicates.True<T>(), iterate);
        }

        public static IEnumerable<T> Generate<T>(T state, Predicate<T> condition, Func<T, T> iterate)
        {
            return EnumerableEx.Generate(state, x => condition(x), iterate, Functions.Identity<T>());
        }
    }
}
