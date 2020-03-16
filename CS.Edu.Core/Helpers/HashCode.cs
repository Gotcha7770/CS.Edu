using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CS.Edu.Core.Helpers
{
    /// <summary>
    /// A hash code used to help with implementing <see cref="object.GetHashCode()"/>.
    /// </summary>
    public struct HashCode : IEquatable<HashCode>
    {
        private const int EmptyCollectionPrimeNumber = 19;
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashCode"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        private HashCode(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="HashCode"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="hashCode">The hash code.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(HashCode hashCode) => hashCode._value;

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(HashCode left, HashCode right) => left.Equals(right);

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(HashCode left, HashCode right) => !(left == right);

        /// <summary>
        /// Takes the hash code of the specified item.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The new hash code.</returns>
        public static HashCode Of<T>(T item) => new HashCode(GetHashCode(item));

        /// <summary>
        /// Takes the hash code of the specified items.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="items">The collection.</param>
        /// <returns>The new hash code.</returns>
        public static HashCode OfEach<T>(IEnumerable<T> items) => items == null
            ? new HashCode(0)
            : new HashCode(GetHashCode(items, 0));

        /// <summary>
        /// Adds the hash code of the specified item.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>The new hash code.</returns>
        public HashCode And<T>(T item) => new HashCode(CombineHashCodes(_value, GetHashCode(item)));

        /// <summary>
        /// Adds the hash code of the specified items in the collection.
        /// </summary>
        /// <typeparam name="T">The type of the items.</typeparam>
        /// <param name="items">The collection.</param>
        /// <returns>The new hash code.</returns>
        public HashCode AndEach<T>(IEnumerable<T> items)
        {
            return items == null
                ? new HashCode(_value)
                : new HashCode(GetHashCode(items, _value));
        }

        /// <inheritdoc />
        public bool Equals(HashCode other)
        {
            return ReferenceEquals(this, other) || _value.Equals(other._value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return obj is HashCode other && Equals(other);
        }

        /// <summary>
        /// Throws <see cref="NotSupportedException" />.
        /// </summary>
        /// <returns>Does not return.</returns>
        /// <exception cref="NotSupportedException">Implicitly convert this struct to an <see cref="int" /> to get the hash code.</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
            throw new NotSupportedException("Implicitly convert this struct to an int to get the hash code.");

        private static int CombineHashCodes(int h1, int h2)
        {
            unchecked
            {
                // Code copied from System.Tuple so it must be the best way to combine hash codes or at least a good one.
                return ((h1 << 5) + h1) ^ h2;
            }
        }

        private static int GetHashCode<T>(T item) => item?.GetHashCode() ?? 0;

        private static int GetHashCode<T>(IEnumerable<T> items, int startHashCode)
        {
            var result = startHashCode;

            using (var enumerator = items.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    result = CombineHashCodes(result, GetHashCode(enumerator.Current));

                    while (enumerator.MoveNext())
                    {
                        result = CombineHashCodes(result, GetHashCode(enumerator.Current));
                    }
                }
                else
                {
                    result = CombineHashCodes(result, EmptyCollectionPrimeNumber);
                }
            }

            return result;
        }
    }
}
