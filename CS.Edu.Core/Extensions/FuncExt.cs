using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Edu.Core.Extensions
{
    public static class FuncExt
    {
        public static Func<A, C> Compose<A, B, C>(Func<A, B> f, Func<B, C> g)
        {
            return (x) => g(f(x));
        }

        static Func<T2, TResult> ApplyPartial<T1, T2, TResult>(Func<T1, T2, TResult> function, T1 arg)
        {
            return (x) => function(arg, x);
        }

        static Func<T1, Func<T2, Func<T3, TResult>>> Curry<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> function)
        {
            return a => b => c => function(a, b, c);
        }
    }
}
