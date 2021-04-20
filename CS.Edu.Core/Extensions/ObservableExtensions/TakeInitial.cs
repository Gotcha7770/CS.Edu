using System;
using System.Reactive.Linq;
using DynamicData;

namespace CS.Edu.Core.Extensions.ObservableExtensions
{
    public static partial class ObservableExt
    {
        public static IObservable<IChangeSet<T>> TakeInitial<T>(this IObservable<IChangeSet<T>> source)
        {
            return source.DeferUntilLoaded().Take(1);
        }

        public static IObservable<IChangeSet<T, TKey>> TakeInitial<T, TKey>(this IObservable<IChangeSet<T, TKey>> source)
        {
            return source.DeferUntilLoaded().Take(1);
        }
    }
}