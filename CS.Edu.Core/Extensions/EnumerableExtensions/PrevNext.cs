using System.Collections.Generic;
using CS.Edu.Core.Iterators;
using DynamicData.Kernel;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class Enumerables
{
    public static IEnumerable<PrevNextValue<T>> ToPrevNextIterator<T>(this IEnumerable<T> source)
    {
        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                yield break;

            Optional<T> previous = Optional<T>.None;
            T current = enumerator.Current;

            while (enumerator.MoveNext())
            {
                T next = enumerator.Current;
                yield return new PrevNextValue<T>(previous, current, next);
                previous = current;
                current = next;
            }

            yield return new PrevNextValue<T>(previous, current, Optional<T>.None);
        }
    }
}