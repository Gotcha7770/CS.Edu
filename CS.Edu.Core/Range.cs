using System;
using System.Diagnostics.CodeAnalysis;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Core
{
    public static class Range
    {
        public static Range<T> Empty<T>() where T : IComparable, IEquatable<T> => Range<T>.Empty;
    }

    public class Range<T> : IEquatable<Range<T>> where T : IComparable, IEquatable<T>
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

        public static bool operator== (Range<T> one, Range<T> other) 
        {
            return Equals(one, other);
        }

        public static bool operator!= (Range<T> one, Range<T> other) 
        {
            return !Equals(one, other);
        }

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
                && other.Minimum.CompareTo(Maximum) < 1;
        }

        public Range<T> Intersection(Range<T> other)
        {
            if (Intersects(other))
            {
                T min = Operators.Max(Minimum, other.Minimum);
                T max = Operators.Min(Maximum, other.Maximum);
                return new Range<T>(min, max);
            }

            return Range.Empty<T>();
        }

        public override bool Equals(object obj)
        {
            return obj is Range<T> other && (ReferenceEquals(this, other) || Equals(other));
        }

        public bool Equals(Range<T> other)
        {
            return Equals(Minimum, other.Minimum)
                    && Equals(Maximum, other.Maximum);
        }        

        public override int GetHashCode()
        {
            int hash = 21;
            hash = (hash * 13) + Minimum.GetHashCode();
            hash = (hash * 13) + Maximum.GetHashCode();

            return hash;
        }
    }
}