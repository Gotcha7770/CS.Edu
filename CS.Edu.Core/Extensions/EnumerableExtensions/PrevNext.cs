﻿using System.Collections.Generic;
using CS.Edu.Core.Iterators;
using DynamicData.Kernel;

namespace CS.Edu.Core.Extensions.EnumerableExtensions
{
    public static partial class EnumerableExt
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
}