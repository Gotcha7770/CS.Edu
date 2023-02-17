using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> Generate<T>(T state, Func<T, T> iterate)
    {
        return Generate(state, Predicates.True<T>(), iterate);
    }

    public static IEnumerable<T> Generate<T>(T state, Predicate<T> condition, Func<T, T> iterate)
    {
        return EnumerableEx.Generate(state, x => condition(x), iterate, Functions.Identity<T>());
    }
}