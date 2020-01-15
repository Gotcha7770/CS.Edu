using System.Collections.Generic;
using CS.Edu.Core.Interfaces;

namespace CS.Edu.Tests
{
    public static class MonoidLaws
    {
        public static bool IsAssociative<MONOID, T>(T x, T y, T z, IEqualityComparer<T> comparer = null)
            where MONOID : struct, IMonoid<T>
        {
            comparer = comparer ?? EqualityComparer<T>.Default;

            return comparer.Equals(Monoid.Concat<MONOID, T>(x, Monoid.Concat<MONOID, T>(y, z)),
                                   Monoid.Concat<MONOID, T>(Monoid.Concat<MONOID, T>(x, y), z));
        }

        public static bool HasIdentity<MONOID, T>(T x, IEqualityComparer<T> comparer = null)
            where MONOID : struct, IMonoid<T>
        {
            comparer = comparer ?? EqualityComparer<T>.Default;

            return comparer.Equals(Monoid.Concat<MONOID, T>(x, Monoid.Empty<MONOID, T>()), x)
                && comparer.Equals(Monoid.Concat<MONOID, T>(Monoid.Empty<MONOID, T>(), x), x);
        }
    }
}