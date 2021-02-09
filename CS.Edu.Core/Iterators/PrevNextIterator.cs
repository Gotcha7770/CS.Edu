using System.Collections.Generic;
using DynamicData.Kernel;

namespace CS.Edu.Core.Iterators
{
    public readonly struct PrevNextValue<T>
    {
        public PrevNextValue(Optional<T> previous, T current, Optional<T> next)
        {
            Previous = previous;
            Current = current;
            Next = next;
        }

        public Optional<T> Previous { get; }

        public T Current { get; }

        public Optional<T> Next { get; }
    }

    public static class PrevNextIteratorExtensions
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