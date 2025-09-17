using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class AsyncEnumerableEx
{
    public static IAsyncEnumerable<T> Memoize<T>(this IAsyncEnumerable<T> source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        throw new NotImplementedException();
    }
}