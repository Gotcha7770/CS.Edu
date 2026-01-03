using System;
using System.Reactive.Linq;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions;

public static partial class Observables
{
    public static IObservable<T> Generate<T>(T state, Func<T, T> iterate)
    {
        return Generate(state, Func<T>.True, iterate);
    }

    public static IObservable<T> Generate<T>(T state, Func<T, bool> condition, Func<T, T> iterate)
    {
        return Observable.Generate(state, condition, iterate, Func<T, T>.Identity);
    }
}