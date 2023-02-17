using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class AsyncEnumerableExtensions
{
    public static async Task<IEnumerable<T>> Sequence<T>(this IEnumerable<Task<T>> source)
    {
        var result = await Task.WhenAll(source);
        return result;
    }

    public static async Task<IEnumerable<TResult>> Traverse<T, TResult>(
        this IEnumerable<T> source,
        Func<T, Task<TResult>> selector)
    {
        var result = await Task.WhenAll(source.Select(selector));
        return result;
    }
}