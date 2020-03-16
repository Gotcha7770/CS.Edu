using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Core
{
    public struct Range<T> : IEquatable<Range<T>> where T : IComparable<T>, IEquatable<T>
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

        public static Range<T> operator &(Range<T> one, Range<T> other)
        {
            return one.Intersection(other);
        }

        public static IEnumerable<Range<T>> operator ^(Range<T> one, Range<T> other)
        {
            return SymmetricDifference(one, other);
        }

        public static IEnumerable<Range<T>> operator -(Range<T> one, Range<T> other)
        {
            return one.Substruct(other);
        }

        public static IEnumerable<Range<T>> SymmetricDifference(Range<T> one, Range<T> other)
        {
            return one.Substruct(other).Concat(other.Substruct(one));
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
                T min = Operators.Max<T>(Min, other.Min);
                T max = Operators.Min<T>(Max, other.Max);
                return new Range<T>(min, max);
            }

            return Default;
        }

        public IEnumerable<Range<T>> Substruct(Range<T> other)
        {
            return DiffIterator(this, other).Where(x => !x.IsEmpty);
        }

        private static IEnumerable<Range<T>> DiffIterator(Range<T> minuend, Range<T> subtrahend)
        {
            //if s.Min < m.Min ⇒ s.Min < m.Max → [s.Min;s.Min] → ∅
            //if m.Max < s.Min ⇒ m.Min < s.Min ⇒ !∀[s.Min;m.Max]
            T _1 = Operators.Min(minuend.Min, subtrahend.Min);
            T _2 = Operators.Min(minuend.Max, subtrahend.Min);

            yield return new Range<T>(_1, _2);

            //if s.Max > m.Max ⇒ s.Max > m.Min → [s.Max;s.Max] → ∅
            //if m.Min > s.Max ⇒ m.Max > s.Max ⇒ !∀[m.Min;s.Max]
            T _3 = Operators.Max(minuend.Min, subtrahend.Max);
            T _4 = Operators.Max(minuend.Max, subtrahend.Max);

            yield return new Range<T>(_3, _4);
        }

        public override bool Equals(object obj)
        {
            return obj is Range<T> other && Equals(other);
        }

        public bool Equals(Range<T> other)
        {
            return ReferenceEquals(this, other) || (Equals(Min, other.Min) && Equals(Max, other.Max));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Min, Max);
        }
    }
}