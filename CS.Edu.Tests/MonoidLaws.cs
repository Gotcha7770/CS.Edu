using CS.Edu.Core.Interfaces;

namespace CS.Edu.Tests
{
    public static class MonoidLaws
    {
        public static bool IsAssociative<MONOID, T>(T x, T y, T z)
            where MONOID : struct, IMonoid<T>
        {
            return Equals(Monoid.Concat<MONOID, T>(x, Monoid.Concat<MONOID, T>(y, z)),
                          Monoid.Concat<MONOID, T>(Monoid.Concat<MONOID, T>(x, y), z));
        }

        public static bool HasIdentity<MONOID, T>(T x)
            where MONOID : struct, IMonoid<T>
        {
            return Equals(Monoid.Concat<MONOID, T>(x, Monoid.Empty<MONOID, T>()), x)
                && Equals(Monoid.Concat<MONOID, T>(Monoid.Empty<MONOID, T>(), x), x);
        }
    }
}