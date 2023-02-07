using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CS.Edu.Core.Extensions.AsyncEnumerableExtensions;

public static partial class AsyncEnumerableExtensions
{
    // public static async IAsyncEnumerable<T> Sequence<T>(this IEnumerable<Task<T>> source)
    // {
    //     foreach (Task<T> task in source)
    //     {
    //         yield return await task;
    //     }
    // }

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