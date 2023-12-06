using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using DynamicData;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class ObservableProductTests
{
    static readonly (int, int)[] Standard =
    {
        (0, 0),
        (0, 1),
        (1, 0),
        (1, 1),
    };

    static readonly int[] Left = {0, 1};
    static readonly int[] Right = {0, 1};

    [Fact]
    public void EnumerableCartesian()
    {
        var cartesian = from x in Left
                        from y in Right
                        select (x, y);

        // foreach(var x in Left)
        // {
        //     foreach (var y in Right)
        //     {
        //         result.Add((x, y));
        //     }
        // }

        //cartesian = Left.SelectMany(x => Right.Select(y => (x, y)))

       cartesian.Should().BeEquivalentTo(Standard);
    }

    [Fact]
    public void ObservableCartesianCold()
    {
        var product = from x in Left.ToObservable()
                      from y in Right.ToObservable()
                      select (x, y);

        var cartesian = product.ToEnumerable();

        cartesian.Should().BeEquivalentTo(Standard);
    }

    [Fact]
    public void ObservableCartesianHot()
    {
        ISubject<int> left = new Subject<int>();
        ISubject<int> right = new Subject<int>();

        var cartesian = new List<(int, int)>();

        var product = from x in left
                      from y in right
                      select (x, y);

        using var subscription = product.Subscribe(x => cartesian.Add(x));
        cartesian.Should().BeEmpty();

        left.OnNext(0);
        left.OnNext(1);

        cartesian.Should().BeEmpty();

        right.OnNext(0);

        cartesian.Should().BeEquivalentTo([(0, 0), (1, 0)]);

        right.OnNext(1);

        cartesian.Should().BeEquivalentTo([(0, 0), (1, 0), (0, 1), (1, 1)]);
    }

    [Fact]
    public void ObservableChangeSetCartesianCold()
    {
        IObservable<IChangeSet<int>> one = Left.AsObservableChangeSet();
        IObservable<IChangeSet<int>> other = Right.AsObservableChangeSet();

        using var subscription = one.Product(other, (x, y) => (x, y))
            .Bind(out ReadOnlyObservableCollection<(int, int)> cartesian)
            .Subscribe();

        var product = from x in one
                      from y in other
                      select (x, y);

        cartesian.Should().BeEquivalentTo(Standard);
    }

    [Fact]
    public void ObservableChangeSetCartesianHot()
    {
        ISourceList<int> left = new SourceList<int>();
        ISourceList<int> right = new SourceList<int>();

        using var subscription = left.Connect()
            .Product(right.Connect(), (l, r) => (l, r))
            .Bind(out ReadOnlyObservableCollection<(int, int)> cartesian)
            .Subscribe();

        cartesian.Should().BeEmpty();

        left.AddRange([0, 1]);

        cartesian.Should().BeEmpty();

        right.Add(0);

        cartesian.Should().BeEquivalentTo([(0, 0), (1, 0)]);

        right.Add(1);

        cartesian.Should().BeEquivalentTo([(0, 0), (1, 0), (0, 1), (1, 1)]);

        left.Remove(0);

        cartesian.Should().BeEquivalentTo([(1, 0), (1, 1)]);

        right.Clear();

        cartesian.Should().BeEmpty();
    }
}