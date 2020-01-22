using System;
using System.Collections.Generic;
using System.Linq;
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
                T min = Operators.Max(Min, other.Min);
                T max = Operators.Min(Max, other.Max);
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
            var _1 = Operators.Min<T>(minuend.Min, subtrahend.Min);
            var _2 = Operators.Min<T>(minuend.Max, subtrahend.Min);

            yield return new Range<T>(_1, _2);

            var _3 = Operators.Max<T>(minuend.Min, subtrahend.Max);
            var _4 = Operators.Max<T>(minuend.Max, subtrahend.Max);

            yield return new Range<T>(_3, _4);
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