using System;
using DynamicData.Kernel;

namespace CS.Edu.Tests.Extensions;

public static class Optionals
{
    public static Optional<T> ValueOr<T>(this Optional<T> source, Func<T> defaultProvider)
    {
        return source.HasValue ? source.Value : defaultProvider();
    }

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
}