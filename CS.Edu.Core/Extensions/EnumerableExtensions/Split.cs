using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

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
    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> source, Relation<T> relation)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (relation == null)
            throw new ArgumentNullException(nameof(relation));

        return SplitIterator(source, relation);
    }

    private static IEnumerable<IEnumerable<T>> SplitIterator<T>(IEnumerable<T> source, Relation<T> relation)
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

    public static IEnumerable<IEnumerable<T>> Split<T>(
        this IEnumerable<T> source,
        Relation<T, T, T> relation,
        SplitOptions options = SplitOptions.None)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (relation == null)
            throw new ArgumentNullException(nameof(relation));

        return options.HasFlag(SplitOptions.IncludeBorders)
            ? SplitWithBordersIterator(source, relation)
            : SplitIterator(source, relation);
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

    static IEnumerable<IEnumerable<T>> SplitWithBordersIterator<T>(IEnumerable<T> source, Relation<T, T, T> relation)
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
}