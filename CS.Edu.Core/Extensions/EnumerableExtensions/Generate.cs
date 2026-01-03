using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> Generate<T>(T state, Func<T, T> iterate)
    {
        return Generate(state, Func<T>.True, iterate);
    }

    public static IEnumerable<T> Generate<T>(T state, Func<T, bool> condition, Func<T, T> iterate)
    {
        return EnumerableEx.Generate(state, condition, iterate, Func<T, T>.Identity);
    }
}