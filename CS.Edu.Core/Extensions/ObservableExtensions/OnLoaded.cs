using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Kernel;

// ReSharper disable once CheckNamespace
namespace CS.Edu.Core.Extensions
{
    public static partial class Observables
    {
        public static IObservable<IChangeSet<T>> OnLoaded<T>(this IObservable<IChangeSet<T>> source, Action<IChangeSet<T>> action)
        {
            return new LoadedMonitor<T>(source, action).Run();
        }

        public static IObservable<IChangeSet<T, TKey>> OnLoaded<T, TKey>(this IObservable<IChangeSet<T, TKey>> source, Action<IChangeSet<T, TKey>> action)
        {
            return new LoadedMonitor<T, TKey>(source, action).Run();
        }
    }

    internal class LoadedMonitor<T>
    {
        private readonly IObservable<IChangeSet<T>> _source;
        private readonly Action<IChangeSet<T>> _action;

        public LoadedMonitor(IObservable<IChangeSet<T>> source, Action<IChangeSet<T>> action)
        {
            _source = source;
            _action = action;
        }

        public IObservable<IChangeSet<T>> Run()
        {
            return Observable.Create<IChangeSet<T>>(observer =>
            {
                using var batchGuard = new BehaviorSubject<bool>(true);
                var loadedObservable = _source.MonitorStatus()
                    .Where(x => x == ConnectionStatus.Loaded);

                using var actionInvoker = _source.SkipUntil(loadedObservable)
                    .BufferIf(batchGuard)
                    .Take(1)
                    .Do(_action)
                    .Subscribe();

                var subscription = _source.Subscribe(observer);
                batchGuard.OnNext(false);

                return subscription;
            });
        }
    }

    internal class LoadedMonitor<T, TKey>
    {
        private readonly IObservable<IChangeSet<T, TKey>> _source;
        private readonly Action<IChangeSet<T, TKey>> _action;

        public LoadedMonitor(IObservable<IChangeSet<T, TKey>> source, Action<IChangeSet<T, TKey>> action)
        {
            _source = source;
            _action = action;
        }

        public IObservable<IChangeSet<T, TKey>> Run()
        {
            return Observable.Create<IChangeSet<T, TKey>>(observer =>
            {
                using var batchGuard = new BehaviorSubject<bool>(true);
                var loadedObservable = _source.MonitorStatus()
                    .Where(x => x == ConnectionStatus.Loaded);

                using var actionInvoker = _source.SkipUntil(loadedObservable)
                    .BatchIf(batchGuard, null)
                    .Take(1)
                    .Do(_action)
                    .Subscribe();

                var subscription = _source.Subscribe(observer);
                batchGuard.OnNext(false);

                return subscription;
            });
        }
    }
}