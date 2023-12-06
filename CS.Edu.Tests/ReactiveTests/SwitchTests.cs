using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using DynamicData;
using DynamicData.Tests;
using FluentAssertions;
using Microsoft.Reactive.Testing;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class SwitchTests
{
    [Fact]
    public void SelectManyTest()
    {
        //SelectMany is just: source.Select(selector).Merge();

        var scheduler = new TestScheduler();
        var one = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 0),
            ReactiveTest.OnNext(20, 0),
            ReactiveTest.OnNext(30, 0),
            ReactiveTest.OnNext(40, 0),
            ReactiveTest.OnCompleted<int>(50));

        var other = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 1),
            ReactiveTest.OnNext(20, 1),
            ReactiveTest.OnNext(30, 1),
            ReactiveTest.OnNext(40, 1),
            ReactiveTest.OnCompleted<int>(50));

        var outer = scheduler.CreateHotObservable(ReactiveTest.OnNext<IObservable<int>>(5, one),
            ReactiveTest.OnNext<IObservable<int>>(15, other),
            ReactiveTest.OnNext<IObservable<int>>(25, one),
            ReactiveTest.OnNext<IObservable<int>>(35, other),
            ReactiveTest.OnCompleted<IObservable<int>>(60));

        var res = scheduler.Start(() => outer.SelectMany(x => x), 0, 0, ReactiveTest.Disposed);

        res.Messages.AssertEqual(
            ReactiveTest.OnNext(10, 0),
            ReactiveTest.OnNext(20, 0),
            ReactiveTest.OnNext(20, 1),
            ReactiveTest.OnNext(30, 0),
            ReactiveTest.OnNext(30, 0),
            ReactiveTest.OnNext(30, 1),
            ReactiveTest.OnNext(40, 0),
            ReactiveTest.OnNext(40, 0),
            ReactiveTest.OnNext(40, 1),
            ReactiveTest.OnNext(40, 1),
            ReactiveTest.OnCompleted<int>(60));
    }

    [Fact]
    public void SwitchTest()
    {
        var scheduler = new TestScheduler();
        var one = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 0),
            ReactiveTest.OnNext(20, 0),
            ReactiveTest.OnNext(30, 0),
            ReactiveTest.OnNext(40, 0),
            ReactiveTest.OnCompleted<int>(50));

        var other = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 1),
            ReactiveTest.OnNext(20, 1),
            ReactiveTest.OnNext(30, 1),
            ReactiveTest.OnNext(40, 1),
            ReactiveTest.OnCompleted<int>(50));

        var outer = scheduler.CreateHotObservable(ReactiveTest.OnNext<IObservable<int>>(5, one),
            ReactiveTest.OnNext<IObservable<int>>(15, other),
            ReactiveTest.OnNext<IObservable<int>>(25, one),
            ReactiveTest.OnNext<IObservable<int>>(35, other),
            ReactiveTest.OnCompleted<IObservable<int>>(60));

        var res = scheduler.Start(() => outer.Switch(), 0, 0, ReactiveTest.Disposed);

        res.Messages.AssertEqual(
            ReactiveTest.OnNext(10, 0),
            ReactiveTest.OnNext(20, 1),
            ReactiveTest.OnNext(30, 0),
            ReactiveTest.OnNext(40, 1),
            ReactiveTest.OnCompleted<int>(60));
    }

    [Fact]
    public void SwitchOnTest()
    {
        ISourceList<int> one = Source.From([1, 3]);
        ISourceList<int> other = Source.From([2, 4]);

        using var switcher = new BehaviorSubject<Unit>(Unit.Default);
        using var aggregate = switcher.Scan(other, (agg, _) => agg == one ? other : one).Switch().AsAggregator();

        aggregate.Data.Items.Should().BeEquivalentTo([1, 3]);

        switcher.OnNext(Unit.Default);

        aggregate.Data.Items.Should().BeEquivalentTo([2, 4]);

        one.Add(5);

        aggregate.Data.Items.Should().BeEquivalentTo([2, 4]);

        switcher.OnNext(Unit.Default);

        aggregate.Data.Items.Should().BeEquivalentTo([1, 3, 5]);

        other.Remove(4);

        aggregate.Data.Items.Should().BeEquivalentTo([1, 3, 5]);

        one.Remove(3);

        aggregate.Data.Items.Should().BeEquivalentTo([1, 5]);
    }
}