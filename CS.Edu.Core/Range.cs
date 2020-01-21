using System;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Core
{
    public struct Range<T> : IEquatable<Range<T>> where T : IComparable, IEquatable<T>
    {
        public static Range<T> Default { get; } = new Range<T>();

        public Range(T minimum, T maximum)
        {
            //if minimum > maximum???
            Min = minimum;
            Max = maximum;
        }

        public T Min { get; }

        public T Max { get; }

        public bool IsDefault => Equals(Default);

        public bool IsEmpty => Min.CompareTo(Max) == 0;

        public static bool operator ==(Range<T> one, Range<T> other)
        {
            return Equals(one, other);
        }

        public static bool operator !=(Range<T> one, Range<T> other)
        {
            return !Equals(one, other);
        }

        public bool Contains(T value)
        {
            return Min.CompareTo(value) < 1
                && Max.CompareTo(value) > -1;
        }

        public bool Contains(Range<T> other)
        {
            return Min.CompareTo(other.Min) < 1
                && Max.CompareTo(other.Max) > -1;
        }

        public bool Intersects(Range<T> other)
        {
            return Min.CompareTo(other.Max) < 1
                && other.Min.CompareTo(Max) < 1;
        }

        public Range<T> Intersection(Range<T> other)
        {
            if (Intersects(other))
            {
                T min = Operators.Max(Min, other.Min);
                T max = Operators.Min(Max, other.Max);
                return new Range<T>(min, max);
            }

            return Default;
        }

        public Range<T>[] Substruct(Range<T> other)
        {
            bool left = Contains(other.Min);
            bool right = Contains(other.Max);

            return (left, right) switch
            {
                (true, true) => Equals(other) ? Array.Empty<Range<T>>() : SubstructFromCenter(other),
                (true, false) => new[] { new Range<T>(Min, other.Min) },
                (false, true) => new[] { new Range<T>(other.Max, Max) },
                _ => other.Contains(this) ? Array.Empty<Range<T>>() : new[] { this }
            };
        }

        private Range<T>[] SubstructFromCenter(Range<T> other)
        {
            return new[]
            {
                new Range<T>(Min, other.Min),
                new Range<T>(other.Max, Max)
            };
        }

        private Range<T> UpperBound(T x, T y)
        {
            return new Range<T>(Operators.Min<T>(x, y), y);
        }

        public override bool Equals(object obj)
        {
            return obj is Range<T> other && (ReferenceEquals(this, other) || Equals(other));
        }

        public bool Equals(Range<T> other)
        {
            return Equals(Min, other.Min)
                    && Equals(Max, other.Max);
        }

        public override int GetHashCode()
        {
            int hash = 21;
            hash = (hash * 13) + Min.GetHashCode();
            hash = (hash * 13) + Max.GetHashCode();

            return hash;
        }
    }
}