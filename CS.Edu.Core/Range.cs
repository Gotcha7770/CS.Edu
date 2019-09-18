using System;

namespace CS.Edu.Core
{
    public static class Range
    {
        public static Range<T> Empty<T>() where T : IComparable => Range<T>.Empty;
    }

    public class Range<T> where T : IComparable
    {
        internal static Range<T> Empty => new Range<T>();

        public Range() : this(default(T), default(T)) { }

        public Range(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public T Minimum { get; }

        public T Maximum { get; }

        public bool Contains(T value)
        {
            return false;
        }

        public bool Contains(Range<T> other)
        {
            return Minimum.CompareTo(other.Minimum) < 1
                && Maximum.CompareTo(other.Maximum) > -1;
        }

        public bool Intersects(Range<T> other)
        {
            return Minimum.CompareTo(other.Maximum) < 1 
                && Maximum.CompareTo(other.Minimum) > -1;
        }

        public Range<T> Intersection(Range<T> other)
        {
            return new Range<T>();
        }
    }
}