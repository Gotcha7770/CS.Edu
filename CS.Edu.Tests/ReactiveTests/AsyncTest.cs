using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Tests.TaskTests;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CS.Edu.Tests.ReactiveTests
{
    public interface IAsyncDisposable
    {
        Task DisposeAsync();
    }

    public interface IAsyncObserver<in T>
    {
        Task OnNextAsync(T value);
        Task OnErrorAsync(Exception error);
        Task OnCompletedAsync();
    }

    public interface IAsyncObservable<out T>
    {
        Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer);
    }

    [TestFixture]
    public class AsyncTest
    {
        //int a = LongOperation();
        //int b = await Task.Run(LongOperation); //doesn't work
        //IObservable<int> c = Observable.Return(LongOperation());
        //var d = Observable.Return(async () => await Task.Run(LongOperation)); //doesn't work

        [Test]
        public void AsyncObservableReturn()
        {
            int result = 0;
            using (var subscription = AsyncObservable.Return(LongOperation())
                .SubscribeAsync(x => result = x))
            {
                Assert.AreEqual(42, result);
            }
        }

        [Test]
        public void AsyncObservableCreate()
        {
            var observable = AsyncObservable.Create<int>(async observer =>
            {
                var result = await Task.Run(LongOperation);
                await observer.OnNextAsync(result);
                await observer.OnCompletedAsync();

                return AsyncDisposable.Nop;
            });

            var tmp = 2;

            int result = 0;
            var subscription = observable.SubscribeAsync(x => result = x);

            Assert.AreEqual(42, result);
        }

        int LongOperation()
        {
            Thread.Sleep(1000);
            return 42;
        }
    }

    public abstract class AsyncObserverBase<T> : IAsyncObserver<T>
    {
        private const int Idle = 0;
        private const int Busy = 1;
        private const int Done = 2;

        private int _status = Idle;

        public Task OnCompletedAsync()
        {
            TryEnter();

            try
            {
                return OnCompletedAsyncCore();
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract Task OnCompletedAsyncCore();

        public Task OnErrorAsync(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            TryEnter();

            try
            {
                return OnErrorAsyncCore(error);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Done);
            }
        }

        protected abstract Task OnErrorAsyncCore(Exception error);

        public Task OnNextAsync(T value)
        {
            TryEnter();

            try
            {
                return OnNextAsyncCore(value);
            }
            finally
            {
                Interlocked.Exchange(ref _status, Idle);
            }
        }

        protected abstract Task OnNextAsyncCore(T value);

        private void TryEnter()
        {
            var old = Interlocked.CompareExchange(ref _status, Busy, Idle);

            switch (old)
            {
                case Busy:
                    throw new InvalidOperationException("The observer is currently processing a notification.");
                case Done:
                    throw new InvalidOperationException("The observer has already terminated.");
            }
        }
    }

    public class AsyncObserver<T> : AsyncObserverBase<T>
    {
        private readonly Func<T, Task> _onNextAsync;
        private readonly Func<Exception, Task> _onErrorAsync;
        private readonly Func<Task> _onCompletedAsync;

        public AsyncObserver(Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            _onNextAsync = onNextAsync;
            _onErrorAsync = onErrorAsync;
            _onCompletedAsync = onCompletedAsync;
        }

        protected override Task OnCompletedAsyncCore() => _onCompletedAsync();

        protected override Task OnErrorAsyncCore(Exception error) => _onErrorAsync(error ?? throw new ArgumentNullException(nameof(error)));

        protected override Task OnNextAsyncCore(T value) => _onNextAsync(value);
    }

    public abstract class AsyncObservableBase<T> : IAsyncObservable<T>
    {
        public async Task<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            var autoDetach = new AutoDetachAsyncObserver(observer);

            var subscription = await SubscribeAsyncCore(autoDetach).ConfigureAwait(false);

            await autoDetach.AssignAsync(subscription);

            return autoDetach;
        }

        protected abstract Task<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer);

        private sealed class AutoDetachAsyncObserver : AsyncObserverBase<T>, IAsyncDisposable
        {
            private readonly IAsyncObserver<T> _observer;
            private readonly object _gate = new object();

            private IAsyncDisposable _subscription;
            private Task _task;
            private bool _disposing;

            public AutoDetachAsyncObserver(IAsyncObserver<T> observer)
            {
                _observer = observer;
            }

            public async Task AssignAsync(IAsyncDisposable subscription)
            {
                var shouldDispose = false;

                lock (_gate)
                {
                    if (_disposing)
                    {
                        shouldDispose = true;
                    }
                    else
                    {
                        _subscription = subscription;
                    }
                }

                if (shouldDispose)
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);
                }
            }

            public async Task DisposeAsync()
            {
                var task = default(Task);
                var subscription = default(IAsyncDisposable);

                lock (_gate)
                {
                    //
                    // NB: The postcondition of awaiting the first DisposeAsync call to complete is that all message
                    //     processing has ceased, i.e. no further On*AsyncCore calls will be made. This is achieved
                    //     here by setting _disposing to true, which is checked by the On*AsyncCore calls upon
                    //     entry, and by awaiting the task of any in-flight On*AsyncCore calls.
                    //
                    //     Timing of the disposal of the subscription is less deterministic due to the intersection
                    //     with the AssignAsync code path. However, the auto-detach observer can only be returned
                    //     from the SubscribeAsync call *after* a call to AssignAsync has been made and awaited, so
                    //     either AssignAsync triggers the disposal and an already disposed instance is returned, or
                    //     the user calling DisposeAsync will either encounter a busy observer which will be stopped
                    //     in its tracks (as described above) or it will trigger a disposal of the subscription. In
                    //     both these cases the result of awaiting DisposeAsync guarantees no further message flow.
                    //

                    if (!_disposing)
                    {
                        _disposing = true;

                        task = _task;
                        subscription = _subscription;
                    }
                }

                try
                {
                    //
                    // BUGBUG: This causes grief when an outgoing On*Async call reenters the DisposeAsync method and
                    //         results in the task returned from the On*Async call to be awaited to serialize the
                    //         call to subscription.DisposeAsync after it's done. We need to either detect reentrancy
                    //         and queue up the call to DisposeAsync or follow an when we trigger the disposal without
                    //         awaiting outstanding work (thus allowing for concurrency).
                    //
                    // if (task != null)
                    // {
                    //     await task.ConfigureAwait(false);
                    // }
                    //
                }
                finally
                {
                    if (subscription != null)
                    {
                        await subscription.DisposeAsync().ConfigureAwait(false);
                    }
                }
            }

            protected override async Task OnCompletedAsyncCore()
            {
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _task = _observer.OnCompletedAsync();
                }

                try
                {
                    await _task.ConfigureAwait(false);
                }
                finally
                {
                    await FinishAsync().ConfigureAwait(false);
                }
            }

            protected override async Task OnErrorAsyncCore(Exception error)
            {
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _task = _observer.OnErrorAsync(error);
                }

                try
                {
                    await _task.ConfigureAwait(false);
                }
                finally
                {
                    await FinishAsync().ConfigureAwait(false);
                }
            }

            protected override async Task OnNextAsyncCore(T value)
            {
                lock (_gate)
                {
                    if (_disposing)
                    {
                        return;
                    }

                    _task = _observer.OnNextAsync(value);
                }

                try
                {
                    await _task.ConfigureAwait(false);
                }
                finally
                {
                    lock (_gate)
                    {
                        _task = null;
                    }
                }
            }

            private async Task FinishAsync()
            {
                var subscription = default(IAsyncDisposable);

                lock (_gate)
                {
                    if (!_disposing)
                    {
                        _disposing = true;

                        subscription = _subscription;
                    }

                    _task = null;
                }

                if (subscription != null)
                {
                    await subscription.DisposeAsync().ConfigureAwait(false);
                }
            }
        }
    }

    public class AsyncObservable<T> : AsyncObservableBase<T>
    {
        private readonly Func<IAsyncObserver<T>, Task<IAsyncDisposable>> _subscribeAsync;

        public AsyncObservable(Func<IAsyncObserver<T>, Task<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            _subscribeAsync = subscribeAsync;
        }

        protected override Task<IAsyncDisposable> SubscribeAsyncCore(IAsyncObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            return _subscribeAsync(observer);
        }
    }

    public static class AsyncObservable
    {
        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, ex => Task.FromException(ex), () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, onErrorAsync, () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Task> onCompletedAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, ex => Task.FromException(ex), onCompletedAsync));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Func<T, Task> onNextAsync, Func<Exception, Task> onErrorAsync, Func<Task> onCompletedAsync)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNextAsync == null)
                throw new ArgumentNullException(nameof(onNextAsync));
            if (onErrorAsync == null)
                throw new ArgumentNullException(nameof(onErrorAsync));
            if (onCompletedAsync == null)
                throw new ArgumentNullException(nameof(onCompletedAsync));

            return source.SubscribeAsync(new AsyncObserver<T>(onNextAsync, onErrorAsync, onCompletedAsync));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => Task.FromException(ex), () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext, Action<Exception> onError)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => { onError(ex); return Task.CompletedTask; }, () => Task.CompletedTask));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => Task.FromException(ex), () => { onCompleted(); return Task.CompletedTask; }));
        }

        public static Task<IAsyncDisposable> SubscribeAsync<T>(this IAsyncObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (onNext == null)
                throw new ArgumentNullException(nameof(onNext));
            if (onError == null)
                throw new ArgumentNullException(nameof(onError));
            if (onCompleted == null)
                throw new ArgumentNullException(nameof(onCompleted));

            return source.SubscribeAsync(new AsyncObserver<T>(x => { onNext(x); return Task.CompletedTask; }, ex => { onError(ex); return Task.CompletedTask; }, () => { onCompleted(); return Task.CompletedTask; }));
        }

        public static IAsyncObservable<T> Create<T>(Func<IAsyncObserver<T>, Task<IAsyncDisposable>> subscribeAsync)
        {
            if (subscribeAsync == null)
                throw new ArgumentNullException(nameof(subscribeAsync));

            return new AsyncObservable<T>(subscribeAsync);
        }

        public static IAsyncObservable<TSource> Return<TSource>(TSource value)
        {
            return Create<TSource>(async observer =>
            {
                await observer.OnNextAsync(value);
                await observer.OnCompletedAsync();

                return AsyncDisposable.Nop;
            });
        }
    }

    public static class AsyncDisposable
    {
        public static IAsyncDisposable Nop { get; } = new NopAsyncDisposable();

        private sealed class NopAsyncDisposable : IAsyncDisposable
        {
            public Task DisposeAsync() => Task.CompletedTask;
        }
    }
}