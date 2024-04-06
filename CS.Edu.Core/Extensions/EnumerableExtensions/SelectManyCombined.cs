using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IEnumerable<TSource> source, Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return SelectManyIterator(source, collectionSelector, resultSelector);
    }

    private static async IAsyncEnumerable<TResult> SelectManyIterator<TSource, TCollection, TResult>(
        IEnumerable<TSource> source,
        Func<TSource, IAsyncEnumerable<TCollection>> collectionSelector,
        Func<TSource, TCollection, TResult> resultSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (TSource element in source)
        {
            await foreach (TCollection subElement in collectionSelector(element).WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return resultSelector(element, subElement);
            }
        }
    }

    public static IAsyncEnumerable<TResult> SelectMany<TSource, TCollection, TResult>(this IAsyncEnumerable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return SelectManyIterator(source, collectionSelector, resultSelector);
    }

    private static async IAsyncEnumerable<TResult> SelectManyIterator<TSource, TCollection, TResult>(
        IAsyncEnumerable<TSource> source,
        Func<TSource, IEnumerable<TCollection>> collectionSelector,
        Func<TSource, TCollection, TResult> resultSelector,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (TSource element in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            foreach (TCollection subElement in collectionSelector(element))
            {
                yield return resultSelector(element, subElement);
            }
        }
    }
}