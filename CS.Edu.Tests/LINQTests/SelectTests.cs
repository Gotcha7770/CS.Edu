using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class SelectTests
{
    [Fact]
    public async Task AsyncSelector()
    {
        var result = await Enumerable.Range(0, 10)
            .Select(ValueTask.FromResult)
            .ToArrayAsync();

        var syncQuery = from i in Enumerable.Range(0, 10)
                        let r = 10
                        select (i, r);

        // lack of await word in async LINQ query
        // var asyncQuery = from i in Enumerable.Range(0, 10).ToAsyncEnumerable()
        //                  let r = await ValueTask.FromResult(i)
        //                  select (r, i);
    }
}

public static partial class AdHocExtensions
{
    //??? How to pass CancellationToken
    public static IAsyncEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(selector);

        return SelectManyIterator(source, selector);
    }

    private static async IAsyncEnumerable<TResult> SelectManyIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, ValueTask<TResult>> selector)
    {
        foreach (TSource element in source)
        {
            yield return await selector(element);
        }
    }
}