using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;

namespace CS.Edu.Core.Extensions
{
    public static class ObservableExt
    {
        public static IObservable<TValue> CreateFromProperty<TSender, TValue>(TSender source,
                                                                              Expression<Func<TSender, TValue>> expression)
            where TSender : INotifyPropertyChanged
        {
            if (!(expression?.Body is MemberExpression memberExpression))
            {
                throw new ArgumentNullException(nameof(expression));
            }

            Func<TSender, TValue> getter = expression.Compile();
            string propertyName = memberExpression.Member.Name;

            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                x => source.PropertyChanged += x,
                x => source.PropertyChanged -= x)
            .Where(x => propertyName.Equals(x.EventArgs.PropertyName, StringComparison.InvariantCulture))
            .Select(x => getter(source));
        }

        private static readonly ListChangeReason[] ProductChangeReason =
        {
            ListChangeReason.Add,
            ListChangeReason.AddRange,
            ListChangeReason.Remove,
            ListChangeReason.RemoveRange,
            ListChangeReason.Clear
        };

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
                    .WhereReasonsAre(ProductChangeReason)
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