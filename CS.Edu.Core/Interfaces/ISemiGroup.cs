using System.Diagnostics.Contracts;

namespace CS.Edu.Core.Interfaces;

public interface ISemigroup<T>
{
    [Pure]
    T Append(T x, T y);
}

public static class Semigroup
{
    [Pure]
    public static T Append<SEMI, T>(T x, T y)
        where SEMI : struct, ISemigroup<T>
    {
        return default(SEMI).Append(x, y);
    }
}