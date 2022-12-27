using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> source, Relation<T> relation)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (relation == null)
            throw new ArgumentNullException(nameof(relation));

        return SkipWhileIterator(source, relation);
    }

    public static IEnumerable<T> SkipWhile<T>(this IEnumerable<T> source, Relation<T, T, T> relation)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (relation == null)
            throw new ArgumentNullException(nameof(relation));

        return SkipWhileIterator(source, relation);
    }

    private static IEnumerable<T> SkipWhileIterator<T>(IEnumerable<T> source, Relation<T> relation)
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

    private static IEnumerable<T> SkipWhileIterator<T>(IEnumerable<T> source, Relation<T, T, T> relation)
    {
        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                yield break;

            T first = enumerator.Current;

            if (!enumerator.MoveNext())
                yield break;

            T second = enumerator.Current;
            bool yielding = false;

            while (enumerator.MoveNext())
            {
                if (!yielding)
                {
                    if (relation(first, second, enumerator.Current))
                    {
                        first = second;
                        second = enumerator.Current;
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
}