using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions
{
    public static partial class Enumerables
    {
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

        private static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source, Relation<T> relation)
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

        private static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source, Relation<T, T, T> relation)
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
    }
}