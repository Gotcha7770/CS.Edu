using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item, EqualityComparer<T> comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;
        return source.Where(x => !comparer.Equals(x, item));
    }
}