using System;
using System.Reactive.Linq;

namespace CS.Edu.Core.Extensions.ObservableExtensions
{
    public static partial class ObservableExt
    {
        public static IObservable<T> Generate<T>(T state, Func<T, T> iterate)
        {
            return Generate(state, Predicates.True<T>(), iterate);
        }

        public static IObservable<T> Generate<T>(T state, Predicate<T> condition, Func<T, T> iterate)
        {
            return Observable.Generate(state, x => condition(x), iterate, Function.Identity<T>());
        }
    }
}