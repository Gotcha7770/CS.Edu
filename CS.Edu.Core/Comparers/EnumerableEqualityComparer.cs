using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CS.Edu.Core.Comparers
{
    public class EnumerableEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        private static readonly Lazy<EnumerableEqualityComparer<T>> Lazy = new Lazy<EnumerableEqualityComparer<T>>();

        public static EnumerableEqualityComparer<T> Instance => Lazy.Value;
        
        public bool Equals([AllowNull] IEnumerable<T> one, [AllowNull] IEnumerable<T> other)
        {
            return ReferenceEquals(one, other) || (one != null && other != null && one.SequenceEqual(other));
        }

        public int GetHashCode([DisallowNull] IEnumerable<T> enumerable)
        {
            unchecked
            {
                return enumerable.Where(x => x != null)
                    .Select(x => x.GetHashCode())
                    .Aggregate(17, (acc, cur) => 23 * acc + cur);
            }
        }
    }
}