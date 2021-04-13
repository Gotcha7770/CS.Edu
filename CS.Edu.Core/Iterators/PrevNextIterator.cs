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
}