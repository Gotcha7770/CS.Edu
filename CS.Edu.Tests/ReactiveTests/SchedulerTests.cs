using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class SchedulerTests
{
    private readonly IObservable<int> _source = Observable.Range(0, 10);

    private class State
    {
        public State(ManualResetEvent reset) => Reset = reset;

        public ManualResetEvent Reset { get; }

        public int Id { get; set; } = -1;
    }

    [Fact]
    public void NewThreadScheduler_Schedule()
    {
        var state1 = new State(new ManualResetEvent(false));
        var state2 = new State(new ManualResetEvent(false));

        NewThreadScheduler.Default.Schedule(state1, (_, st) =>
        {
            st.Id = Environment.CurrentManagedThreadId;
            st.Reset.Set();
            return Disposable.Empty;
        });

        NewThreadScheduler.Default.Schedule(state2, (_, st) =>
        {
            st.Id = Environment.CurrentManagedThreadId;
            st.Reset.Set();
            return Disposable.Empty;
        });

        WaitHandle.WaitAll([state1.Reset, state2.Reset]);
        state1.Id.Should().NotBe(state2.Id);
    }

    [Fact]
    public void NewThreadScheduler_ScheduleLongRunning()
    {
        var state1 = new State(new ManualResetEvent(false));
        var state2 = new State(new ManualResetEvent(false));

        NewThreadScheduler.Default.ScheduleLongRunning(state1, (st, _) =>
        {
            st.Id = Environment.CurrentManagedThreadId;
            st.Reset.Set();
        });

        NewThreadScheduler.Default.ScheduleLongRunning(state2, (st, _) =>
        {
            st.Id = Environment.CurrentManagedThreadId;
            st.Reset.Set();
        });

        WaitHandle.WaitAll([state1.Reset, state2.Reset]);
        state1.Id.Should().NotBe(state2.Id);
    }

    [Fact]
    public async Task ObserveOn_NewThreadScheduler_ShortTasksExecutesAsOne()
    {
        var res = await _source.ObserveOn(NewThreadScheduler.Default)
            .Select(x => (x, ThreadId: Environment.CurrentManagedThreadId))
            .ToArray();

        res.Should().HaveCount(10);
        res.Should().OnlyContain(x => x.ThreadId == res[0].ThreadId);
    }

    [Fact]
    public async Task ObserveOn_NewThreadScheduler_()
    {
        var res = await _source.ObserveOn(NewThreadScheduler.Default)
            .Do(_ => Thread.Sleep(TimeSpan.FromMilliseconds(20)))
            .Select(x => (x, ThreadId: Environment.CurrentManagedThreadId))
            .ToArray();

        res.Should().HaveCount(10);
        res.Should().OnlyContain(x => x.ThreadId == res[0].ThreadId);
    }
}