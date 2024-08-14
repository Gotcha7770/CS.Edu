using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CS.Edu.Core.Monads;

public static class BindOverAwaiter
{
    //async/await — это способ последовательно вызвать некий набор функций внутри некоторого контекста

    [AsyncMethodBuilder(typeof(BindableMethodBuilder<>))]
    public class Bindable<T> : INotifyCompletion
    {
        public Bindable(T value) => Value = value;

        internal Bindable() { }

        public T Value { get; private set; }

        public Bindable<T> GetAwaiter() => this;

        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation) => continuation();

        public T GetResult()
        {
            if (!IsCompleted)
                throw new Exception("Not completed");

            return Value;
        }

        internal void SetResult(T result)
        {
            Value = result;
            IsCompleted = true;
        }
    }

    public sealed class BindableMethodBuilder<T>
    {
        public static BindableMethodBuilder<T> Create() => new();

        public Bindable<T> Task { get; } = new();

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
            => stateMachine.MoveNext();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void SetException(Exception exception) { }

        public void SetResult(T result) => Task.SetResult(result);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => GenericAwaitOnCompleted(ref awaiter, ref stateMachine);

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
            => GenericAwaitOnCompleted(ref awaiter, ref stateMachine);

        private void GenericAwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
        }
    }

    public static async void Example()
    {
        var bindable = new Bindable<int>(42);

        var b1 = (await bindable).ToString();
        var b2 = (await bindable).ToString();
        //return b1 + b2;
    }

    public static Bindable<int> BindableExample() => new(42);
    public static async Bindable<int> BindableExampleAsync() => await new Bindable<int>(42);

    public static Task<int> TaskExample() => Task.FromResult(42);
    public static async Task<int> TaskExampleAsync() => await Task.FromResult(42);
}