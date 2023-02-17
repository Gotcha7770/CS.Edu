using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> DefaultIfEmpty<T>(this IEnumerable<T> source, Func<T> defaultProvider)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (defaultProvider is null)
        {
            throw new ArgumentNullException(nameof(defaultProvider));
        }

        return Iterator(source, defaultProvider);
    }

    private static IEnumerable<T> Iterator<T>(IEnumerable<T> source, Func<T> defaultProvider)
    {
        using var enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
        {
            yield return defaultProvider();
            yield break;
        }

        yield return enumerator.Current;

        while (enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}