using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

class HotObject : IObservable<long>, IDisposable
{
    private readonly ISubject<long> _subject = new Subject<long>();
    private readonly IDisposable _cleanup;
    private long _index;

    public HotObject(TimeSpan period, IScheduler scheduler = null)
    {
        scheduler ??= Scheduler.Default;
        _cleanup = scheduler.SchedulePeriodic(this, period, static @this => @this.Tick());
    }

    private void Tick()
    {
        var count = _index;
        _index = unchecked(count + 1);

        _subject.OnNext(count);
    }

    public IDisposable Subscribe(IObserver<long> observer) => _subject.Subscribe(observer);

    public void Dispose() => _cleanup.Dispose();
}

public class ColdHotTests
{
    // Lazy evaluation is good for on-demand queries whereas eager evaluation is good for sharing sequences
    // so as to avoid re-evaluating multiple times.
    // Implementations of IObservable<T> can exhibit similar variations in style.
    // See: http://introtorx.com/Content/v1.0.10621.0/14_HotAndColdObservables.html#HotAndCold

    [Fact]
    public void ColdObservable()
    {
        // Sequences that are passive and start producing notifications on request (when subscribed to)
        // This is typical of cold observables; calling the method does nothing.
        // Subscribing to the returned IObservable<T> will however invoke the create delegate.

        var result1 = new List<long>();
        var result2 = new List<long>();
        var scheduler = new TestScheduler();
        var observable = Observable.Interval(TimeSpan.FromSeconds(1), scheduler).Take(5);

        using var s1 = observable.Subscribe(result1.Add);
        scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
        using var s2 = observable.Subscribe(result2.Add);
        scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);

        result1.Should().BeEquivalentTo(new long[] { 0, 1, 2, 3, 4 });
        result2.Should().BeEquivalentTo(new long[] { 0, 1, 2 });
    }

    [Fact]
    public void HotObservable()
    {
        //Sequences that are active and produce notifications regardless of subscriptions.

        var result1 = new List<long>();
        var result2 = new List<long>();
        var scheduler = new TestScheduler();
        var hotObject = new HotObject(TimeSpan.FromSeconds(1), scheduler);

        scheduler.AdvanceBy(TimeSpan.TicksPerSecond);
        using var s1 = hotObject.Subscribe(result1.Add);
        scheduler.AdvanceBy(TimeSpan.TicksPerSecond);
        using var s2 = hotObject.Subscribe(result2.Add);
        scheduler.AdvanceBy(TimeSpan.TicksPerSecond);

        result1.Should().BeEquivalentTo(new long[] { 1, 2 });
        result2.Should().BeEquivalentTo(new long[] { 2 });
    }

    [Fact]
    public void Publish()
    {
        //If we want to be able to share the actual data values and not just the observable instance,
        //we can use the Publish() extension method.

        var result1 = new List<long>();
        var result2 = new List<long>();
        var scheduler = new TestScheduler();
        var connectable = Observable.Interval(TimeSpan.FromSeconds(1), scheduler).Take(5).Publish();
        connectable.Connect();

        using var s1 = connectable.Subscribe(result1.Add);
        scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
        using var s2 = connectable.Subscribe(result2.Add);
        scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);

        result1.Should().BeEquivalentTo(new long[] { 0, 1, 2, 3, 4 });
        result2.Should().BeEquivalentTo(new long[] { 3, 4 });
    }
}