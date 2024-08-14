using System;
using System.Runtime.CompilerServices;
using CS.Edu.Core;
using Xunit;

namespace CS.Edu.Tests.TaskTests;

public class CustomAwaitableType
{
    [Fact]
    public async void Test()
    {
        await FooAsync();
        RangeParameters rangeParameters = RangeParameters.IncludeBoth;
    }

    private async TaskLike FooAsync()
    {
        await default(TaskLike);
    }

    [AsyncMethodBuilder(typeof(TaskLikeMethodBuilder))]
    public struct TaskLike
    {
        public TaskLikeAwaiter GetAwaiter() => default;
    }

    public struct TaskLikeAwaiter : INotifyCompletion
    {
        public void GetResult() { }

        public bool IsCompleted => true;

        public void OnCompleted(Action continuation) { }
    }

    public sealed class TaskLikeMethodBuilder
    {
        public TaskLikeMethodBuilder() => Console.WriteLine(".ctor");

        public static TaskLikeMethodBuilder Create() => new();

        public void SetStateMachine(IAsyncStateMachine stateMachine) { }

        public void SetResult() => Console.WriteLine("SetResult");

        public void SetException(Exception exception) { }

        public void Start<TStateMachine>(ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            Console.WriteLine("Start");
            stateMachine.MoveNext();
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
        }

        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter,
            ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
        }

        public TaskLike Task => default(TaskLike);
    }
}