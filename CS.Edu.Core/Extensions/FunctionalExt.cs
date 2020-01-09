using System;

namespace CS.Edu.Core.Extensions
{
    public static class FunctionalExt
    {
        public static Func<A, C> Compose<A, B, C>(this Func<A, B> f, Func<B, C> g)
        {
            return (x) => g(f(x));
        }

        public static Func<T2, TResult> ApplyPartial<T1, T2, TResult>(this Func<T1, T2, TResult> function, T1 arg)
        {
            return (x) => function(arg, x);
        }

        public static Action<T2> ApplyPartial<T1, T2>(this Action<T1, T2> action, T1 arg)
        {
            return (x) => action(arg, x);
        }

        public static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> function)
        {
            return a => b => c => function(a, b, c);
        }
    }
}
