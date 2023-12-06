using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class SharedSourceListFact
{
    [Fact]
    public void Aggregate_ReferenceTypeAccumulatorProblem()
    {
        var source = new Subject<IChangeSet<int>>();
        var target1 = new BehaviorSubject<IList<int>>(null);
        var target2 = new BehaviorSubject<IList<int>>(null);

        var observable = source.Aggregate(new List<int>(),
            (acc, changes) =>
            {
                acc.Clone(changes);
                return acc;
            });

        using var s1 = observable.Subscribe(target1);
        using var s2 = observable.Subscribe(target2);

        source.OnNext(new ChangeSet<int> { new Change<int>(ListChangeReason.Add, 1, 0) });
        source.OnCompleted();

        // we expect, target1 contains single value, but it is not
        // because of shared list reference in observable
        target1.Value.Should().BeEquivalentTo([1, 1]);
        target2.Value.Should().BeEquivalentTo([1, 1]);
    }

    [Fact]
    public void Aggregate_Fix1()
    {
        var source = new Subject<IChangeSet<int>>();
        var target1 = new BehaviorSubject<IList<int>>(null);
        var target2 = new BehaviorSubject<IList<int>>(null);

        var observable = Observable.Create<IList<int>>(observer =>
        {
            return source.Aggregate(new List<int>(),
                    (acc, changes) =>
                    {
                        acc.Clone(changes);
                        return acc;
                    })
                .Subscribe(observer);
        });

        using var s1 = observable.Subscribe(target1);
        using var s2 = observable.Subscribe(target2);

        source.OnNext(new ChangeSet<int> { new Change<int>(ListChangeReason.Add, 1, 0) });
        source.OnCompleted();

        target1.Value.Should().BeEquivalentTo([1]);
        target2.Value.Should().BeEquivalentTo([1]);
    }

    [Fact]
    public void Aggregate_Fix2()
    {
        var source = new Subject<IChangeSet<int>>();
        var target1 = new BehaviorSubject<IList<int>>(null);
        var target2 = new BehaviorSubject<IList<int>>(null);

        var observable = source.Aggregate(new List<int>(),
            (acc, changes) =>
            {
                var copy = acc.ToList();
                copy.Clone(changes);
                return copy;
            });

        using var s1 = observable.Subscribe(target1);
        using var s2 = observable.Subscribe(target2);

        source.OnNext(new ChangeSet<int> { new Change<int>(ListChangeReason.Add, 1, 0) });
        source.OnCompleted();

        target1.Value.Should().BeEquivalentTo([1]);
        target2.Value.Should().BeEquivalentTo([1]);
    }

    [Fact]
    public void Scan()
    {
        var source = new Subject<IChangeSet<int>>();
        var target1 = new BehaviorSubject<IList<int>>(null);
        var target2 = new BehaviorSubject<IList<int>>(null);

        var observable = source.Scan(new List<int>(),
            (list, changes) =>
            {
                list.Clone(changes);
                return list;
            });

        using var s1 = observable.Subscribe(target1);
        using var s2 = observable.Subscribe(target2);

        source.OnNext(new ChangeSet<int> { new Change<int>(ListChangeReason.Add, 1, 0) });

        target1.Value.Should().BeEquivalentTo([1, 1]);
        target2.Value.Should().BeEquivalentTo([1, 1]);
    }

    [Fact]
    public void QueryWhenChanged()
    {
        var source = new SourceList<int>();
        var observable = source.Connect()
            .QueryWhenChanged();

        var first = new BehaviorSubject<IReadOnlyCollection<int>>(null);
        using var s1 = observable.Subscribe(first);

        var second = new BehaviorSubject<IReadOnlyCollection<int>>(null);
        using var s2 = observable.Subscribe(second);

        source.Add(1);

        // fixed
        // first.Value.Should().BeEquivalentTo(new[] { 1, 1 });
        // second.Value.Should().BeEquivalentTo(new[] { 1, 1 });
    }
}