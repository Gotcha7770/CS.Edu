using System;
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