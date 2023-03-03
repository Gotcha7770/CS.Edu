using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace CS.Edu.Core.Interfaces;

public interface IMonoid<T> : ISemigroup<T>
{
    [Pure]
    T Empty();
}

public static class Monoid
{
    public static T Empty<MONOID, T>()
        where MONOID : struct, IMonoid<T>
    {
        return default(MONOID).Empty();
    }

    public static T Concat<MONOID, T>(IEnumerable<T> xs)
        where MONOID : struct, IMonoid<T>
    {
        return xs.Aggregate(Empty<MONOID, T>(), (s, x) => Semigroup.Append<MONOID, T>(s, x));
    }

    public static T Concat<MONOID, T>(params T[] xs)
        where MONOID : struct, IMonoid<T>
    {
        return xs.Aggregate(Empty<MONOID, T>(), (s, x) => Semigroup.Append<MONOID, T>(s, x));
    }
}