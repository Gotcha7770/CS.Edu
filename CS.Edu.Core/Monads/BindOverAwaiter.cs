using System;
using System.Runtime.CompilerServices;

namespace CS.Edu.Core.Monads
{
    public static class BindOverAwaiter
    {
        //async/await — это способ последовательно вызвать некий набор функций внутри некоторого контекста

        [AsyncMethodBuilder(typeof(BindableMethodBuilder<>))]
        class Bindable<T> : INotifyCompletion
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
            }
        }

        class BindableMethodBuilder<T>
        {
            public static void Create() => new BindableMethodBuilder<T>();

            public BindableMethodBuilder() => Task = new Bindable<T>();

            public Bindable<T> Task { get; private set; }

            public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
                => stateMachine.MoveNext();

            public void SetStateMachine(IAsyncStateMachine stateMachine) { }
            public void SetException(Exception exception) {}

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

            private void GenericAwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
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
            var result = b1 + b2;
        }
    }
}