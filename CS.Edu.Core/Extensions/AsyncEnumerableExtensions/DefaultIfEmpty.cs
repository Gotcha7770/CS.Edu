using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class AsyncEnumerableEx
{
    public static IAsyncEnumerable<T> DefaultIfEmpty<T>(this IAsyncEnumerable<T> source, Func<T> defaultProvider)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (defaultProvider is null)
        {
            throw new ArgumentNullException(nameof(defaultProvider));
        }

        return Create(ct => Iterator(source, defaultProvider, ct));
    }

    public static IAsyncEnumerable<T> DefaultIfEmptyAwait<T>(this IAsyncEnumerable<T> source,
        Func<ValueTask<T>> defaultProvider)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (defaultProvider is null)
        {
            throw new ArgumentNullException(nameof(defaultProvider));
        }

        return Create(ct => Iterator(source, defaultProvider, ct));
    }

    private static async IAsyncEnumerator<T> Iterator<T>(
        IAsyncEnumerable<T> source,
        Func<T> defaultProvider,
        CancellationToken cancellationToken)
    {
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);

        if (await enumerator.MoveNextAsync())
        {
            do yield return enumerator.Current;
            while (await enumerator.MoveNextAsync());
        }
        else
        {
            yield return defaultProvider();
        }
    }

    private static async IAsyncEnumerator<T> Iterator<T>(IAsyncEnumerable<T> source, Func<ValueTask<T>> defaultProvider,
        CancellationToken cancellationToken)
    {
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);

        if (await enumerator.MoveNextAsync())
        {
            do yield return enumerator.Current;
            while (await enumerator.MoveNextAsync());
        }
        else
        {
            yield return await defaultProvider();
        }
    }
}