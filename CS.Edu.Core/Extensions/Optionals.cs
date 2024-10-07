using System;
using System.Collections.Generic;
using System.Linq;
using DynamicData.Kernel;

namespace CS.Edu.Core.Extensions;

public static class Optionals
{
    public static Optional<T> ValueOr<T>(this Optional<T> source, Func<Optional<T>> defaultProvider)
    {
        return source.HasValue ? source.Value : defaultProvider();
    }

    public static Optional<TResult> Select<T, TResult>(this Optional<T> source, Func<T, TResult> selector)
    {
        return source.HasValue
            ? selector(source.Value)
            : Optional<TResult>.None;
    }

    public static Optional<TResult> SelectMany<T, TResult>(this Optional<T> source, Func<T, Optional<TResult>> selector)
    {
        return source.HasValue
            ? selector(source.Value)
            : Optional<TResult>.None;
    }

    public static Optional<TResult> SelectMany<T, TIntermediate, TResult>(
        this Optional<T> source,
        Func<T, Optional<TIntermediate>> selector,
        Func<T, TIntermediate, TResult> resultSelector)
    {
        if (source.HasValue)
        {
            var intermediate = selector(source.Value);
            if (intermediate.HasValue)
            {
                return resultSelector(source.Value, intermediate.Value);
            }
        }

        return Optional<TResult>.None;
    }

    public static IEnumerable<T> AsEnumerable<T>(this Optional<T> source)
    {
        if (source.HasValue)
            yield return source.Value;
    }

    public static IEnumerable<TResult> SelectMany<T, TCollection, TResult>(
        this Optional<T> source,
        Func<Optional<T>, IEnumerable<TCollection>> collectionSelector,
        Func<Optional<T>, TCollection, TResult> resultSelector)
    {

        if (source.HasValue)
        {
            foreach (TCollection value in collectionSelector(source.Value))
            {
                yield return resultSelector(source, value);
            }
        }
    }
}