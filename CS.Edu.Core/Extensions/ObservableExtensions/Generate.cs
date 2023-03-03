using System;
using System.Reactive.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class Observables
{
    public static IObservable<T> Generate<T>(T state, Func<T, T> iterate)
    {
        return Generate(state, Predicates.True<T>(), iterate);
    }

    public static IObservable<T> Generate<T>(T state, Predicate<T> condition, Func<T, T> iterate)
    {
        return Observable.Generate(state, x => condition(x), iterate, Functions.Identity<T>());
    }
}