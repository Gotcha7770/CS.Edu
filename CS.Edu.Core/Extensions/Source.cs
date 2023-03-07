using System;
using System.Collections.Generic;
using DynamicData;

namespace CS.Edu.Core.Extensions;

public static class Source
{
    public static ISourceList<T> From<T>(IEnumerable<T> items)
    {
        var result = new SourceList<T>();
        result.AddRange(items);

        return result;
    }

    public static ISourceCache<T, TKey> From<T, TKey>(IEnumerable<T> items, Func<T, TKey> keySelector)
    {
        var result = new SourceCache<T, TKey>(keySelector);
        foreach (T item in items)
        {
            result.AddOrUpdate(item);
        }

        return result;
    }
}