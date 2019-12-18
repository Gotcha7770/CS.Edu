using System;

namespace CS.Edu.Core
{
    public struct Indexed<T> : IEquatable<Indexed<T>>
    {
        public Indexed(int index, T value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public T Value { get; }

        public override bool Equals(object obj)
        {
            return obj is Indexed<T> other && (ReferenceEquals(this, other) || Equals(other));
        }

        public bool Equals(Indexed<T> other)
        {
            return Index == other.Index 
                && Value.Equals(Value);
        }        

        public override int GetHashCode()
        {
            int hash = 21;
            hash = (hash * 13) + Index;
            hash = (hash * 13) + Value.GetHashCode();

            return hash;
        }
    }
}