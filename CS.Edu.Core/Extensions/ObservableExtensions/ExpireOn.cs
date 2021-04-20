using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;

namespace CS.Edu.Core.Extensions.ObservableExtensions
{
    public static partial class ObservableExt
    {
        public static IObservable<IChangeSet<T, TKey>> ExpireOn<T, TKey>(this IObservable<IChangeSet<T, TKey>> source, IObservable<Unit> evaluator)
        {
            return new ObservableExpirer<T, TKey>(source, evaluator).Run();
        }
    }

    internal class ObservableExpirer<T, TKey>
    {
        private readonly IObservable<IChangeSet<T, TKey>> _source;
        private readonly IObservable<Unit> _evaluator;

        public ObservableExpirer(IObservable<IChangeSet<T, TKey>> source, IObservable<Unit> evaluator)
        {
            _source = source;
            _evaluator = evaluator;
        }

        public IObservable<IChangeSet<T, TKey>> Run()
        {
            return Observable.Create<IChangeSet<T, TKey>>(observer =>
            {
                var cache = new IntermediateCache<T, TKey>(_source);

                var published = cache.Connect()
                    .Publish();
                var subscriber = published.SubscribeSafe(observer);

                var remover = _evaluator.Finally(observer.OnCompleted)
                    .Subscribe(_ =>
                    {
                        try
                        {
                            cache.Clear();
                        }
                        catch (Exception ex)
                        {
                            observer.OnError(ex);
                        }
                    });

                var connected = published.Connect();

                return new CompositeDisposable
                {
                    connected,
                    subscriber,
                    remover,
                    cache
                };
            });
        }
    }
}