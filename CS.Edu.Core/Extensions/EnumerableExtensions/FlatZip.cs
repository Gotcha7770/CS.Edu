using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class EnumerableExtensions
{
    public static IEnumerable<T> FlatZip<T>(this IEnumerable<T> left, IEnumerable<T> right)
    {
        using (var leftEnumerator = left.GetEnumerator())
        using (var rightEnumerator = right.GetEnumerator())
        {
            while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
            {
                yield return leftEnumerator.Current;
                yield return rightEnumerator.Current;
            }
        }
    }
}