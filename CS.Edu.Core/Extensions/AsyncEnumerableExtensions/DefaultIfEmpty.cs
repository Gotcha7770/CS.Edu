using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class AsyncEnumerableExtensions
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

        return AsyncEnumerable.Create(ct => Iterator(source, defaultProvider, ct));
    }

    public static IAsyncEnumerable<T> DefaultIfEmptyAwait<T>(this IAsyncEnumerable<T> source, Func<ValueTask<T>> defaultProvider)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (defaultProvider is null)
        {
            throw new ArgumentNullException(nameof(defaultProvider));
        }

        return AsyncEnumerable.Create(ct => Iterator(source, defaultProvider, ct));
    }

    private static async IAsyncEnumerator<T> Iterator<T>(
        IAsyncEnumerable<T> source,
        Func<T> defaultProvider,
        CancellationToken cancellationToken)
    {
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);

        if (! await enumerator.MoveNextAsync(cancellationToken))
        {
            yield return defaultProvider();
            yield break;
        }

        yield return enumerator.Current;

        while (await enumerator.MoveNextAsync(cancellationToken))
        {
            yield return enumerator.Current;
        }
    }

    private static async IAsyncEnumerator<T> Iterator<T>(IAsyncEnumerable<T> source, Func<ValueTask<T>> defaultProvider, CancellationToken cancellationToken)
    {
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);

        if (! await enumerator.MoveNextAsync(cancellationToken))
        {
            yield return await defaultProvider();
            yield break;
        }

        yield return enumerator.Current;

        while (await enumerator.MoveNextAsync(cancellationToken))
        {
            yield return enumerator.Current;
        }
    }
}