using System.Collections.Generic;

namespace CS.Edu.Core.Iterators
{
    public readonly struct PrevNextValue<T>
    {
        public PrevNextValue(Option<T> previous, T current, Option<T> next)
        {
            Previous = previous;
            Current = current;
            Next = next;
        }

        public Option<T> Previous { get; }

        public T Current { get; }

        public Option<T> Next { get; }
    }

    public static class PrevNextIteratorExtensions
    {
        public static IEnumerable<PrevNextValue<T>> ToPrevNextIterator<T>(this IEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                Option<T> previous = Option.None;
                T current = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    T next = enumerator.Current;
                    yield return new PrevNextValue<T>(previous, current, next);
                    previous = current;
                    current = next;
                }

                yield return new PrevNextValue<T>(previous, current, Option.None);
            }
        }
    }
}