using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Comparers
{
    public class EnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private static readonly Lazy<EnumerableEqualityComparer<T>> Lazy = new Lazy<EnumerableEqualityComparer<T>>();

        public static EnumerableEqualityComparer<T> Instance => Lazy.Value;
        
        public bool Equals(IEnumerable<T> one, IEnumerable<T> other)
        {
            return ReferenceEquals(one, other) || (one != null && other != null && one.SequenceEqual(other));
        }

        public int GetHashCode(IEnumerable<T> enumerable)
        {
            var result = new HashCode();

            foreach (var item in enumerable)
            {
                result.Add(item);
            }

            return result.ToHashCode();
        }
    }
}