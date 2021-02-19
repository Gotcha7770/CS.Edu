using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using CS.Edu.Core.Comparers;
using DynamicData;
using DynamicData.Kernel;

namespace CS.Edu.Core.Extensions
{
    public static class ObservableExt
    {
        private readonly struct ProductChangeReasons
        {
            public static readonly ListChangeReason[] ForList =
            {
                ListChangeReason.Add,
                ListChangeReason.AddRange,
                ListChangeReason.Remove,
                ListChangeReason.RemoveRange,
                ListChangeReason.Clear
            };

            public static readonly ChangeReason[] ForCache =
            {
                ChangeReason.Add,
                ChangeReason.Update, //???
                ChangeReason.Remove,
            };
        }

        public static IObservable<IChangeSet<TOut>> Product<TIn, TOut>(this IObservable<IChangeSet<TIn>> one,
            IObservable<IChangeSet<TIn>> other,
            Func<TIn, TIn, TOut> productSelector)
        {
            return ObservableChangeSet.Create<TOut>(list =>
            {
                var left = one.Bind(out ReadOnlyObservableCollection<TIn> leftCache)
                    .Subscribe();

                var right = other.Bind(out ReadOnlyObservableCollection<TIn> rightCache)
                    .Subscribe();

                var subscription = one.Merge(other)
                    .WhereReasonsAre(ProductChangeReasons.ForList)
                    .Subscribe(c =>
                    {
                        var all = from x in leftCache
                                  from y in rightCache
                                  select productSelector(x, y);

                        list.EditDiff(all);
                    });

                return new CompositeDisposable
                {
                    subscription,
                    left,
                    right
                };
            });
        }

        public static IObservable<IChangeSet<TOut, TOutKey>> Product<TIn, TOut, TInKey, TOutKey>(this IObservable<IChangeSet<TIn, TInKey>> one,
            IObservable<IChangeSet<TIn, TInKey>> other,
            Func<TIn, TIn, TOut> productSelector,
            Func<TOut, TOutKey> keySelector)
        {
            return ObservableChangeSet.Create(cache =>
            {
                var left = one.Bind(out ReadOnlyObservableCollection<TIn> leftCache)
                    .Subscribe();

                var right = other.Bind(out ReadOnlyObservableCollection<TIn> rightCache)
                    .Subscribe();

                var subscription = one.Merge(other)
                    .WhereReasonsAre(ProductChangeReasons.ForCache)
                    .Subscribe(c =>
                    {
                        var all = from x in leftCache
                            from y in rightCache
                            select productSelector(x, y);

                        var comparer = new GenericEqualityComparer<TOut, TOutKey>(keySelector);
                        cache.EditDiff(all, comparer);
                    });

                return new CompositeDisposable
                {
                    subscription,
                    left,
                    right
                };
            }, keySelector);
        }

        public static IObservable<IChangeSet<T, TKey>> TakeInitial<T, TKey>(this IObservable<IChangeSet<T, TKey>> source)
        {
            return source.DeferUntilLoaded().Take(1);
        }

        public static IObservable<IChangeSet<T>> OnLoaded<T>(this IObservable<IChangeSet<T>> source,
            Action<IChangeSet<T>> action)
        {
            return Observable.Create<IChangeSet<T>>(observer =>
            {
                var pendingHandler = source
                    .MonitorStatus()
                    .Where(x => x == ConnectionStatus.Loaded);

                var actionInvoker = source.SkipUntil(pendingHandler)
                    .Take(1)
                    .Do(action)
                    .Subscribe();

                var subscription = source.Subscribe(observer);

                actionInvoker.Dispose();

                return subscription;
            });
        }

        public static IObservable<IChangeSet<T, TKey>> OnLoaded<T, TKey>(this IObservable<IChangeSet<T, TKey>> source,
            Action<IChangeSet<T, TKey>> action)
        {
            return Observable.Create<IChangeSet<T, TKey>>(observer =>
            {
                var pendingHandler = source
                    .MonitorStatus()
                    .Where(x => x == ConnectionStatus.Loaded);

                var actionInvoker = source.SkipUntil(pendingHandler)
                    .Take(1)
                    .Do(action)
                    .Subscribe();

                var subscription = source.Subscribe(observer);

                actionInvoker.Dispose();

                return subscription;
            });
        }

        public static IObservable<T> Generate<T>(T state, Func<T, T> iterate)
        {
            return Generate(state, Predicates.True<T>(), iterate);
        }

        public static IObservable<T> Generate<T>(T state, Predicate<T> condition, Func<T, T> iterate)
        {
            return Observable.Generate(state, x => condition(x), iterate, Function.Identity<T>());
        }

        public static IObservable<IChangeSet<T>> Tail<T>(this IObservable<IChangeSet<T>> source,
                                                         int numberOfItems)
        {
            return Observable.Create<IChangeSet<T>>(observer =>
            {
                var request = new TailRequest<T>(source, numberOfItems);

                return new CompositeDisposable
                {
                    request,
                    source.Virtualise(Observable.Return(request)).SubscribeSafe(observer)
                };
            });
        }

        public class TailRequest<T> : IVirtualRequest, IDisposable
        {
            private readonly IDisposable _subscription;
            private int _count;

            public TailRequest(IObservable<IChangeSet<T>> source, int numberOfItems)
            {
                _subscription = source.Subscribe(RefreshStartIndex);
                Size = numberOfItems;
            }

            public int Size { get; }

            public int StartIndex => _count > Size ? _count - Size : 0;

            private void RefreshStartIndex(IChangeSet<T> changeSet)
            {
                _count += changeSet.Adds;
                _count -= changeSet.Removes;
            }

            public void Dispose()
            {
                _subscription.Dispose();
            }
        }
    }
}