using System;
using System.Linq;
using System.Reactive.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using CS.Edu.Tests.Utils.Models;
using DynamicData;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class LastChangedTests
{
    [Fact]
    public void LastOrDefaultTest()
    {
        int lastOrDefault = 0;
        var source = new SourceList<int>();

        using var autoSelector = source.Connect()
            .ToCollection()
            .Subscribe(items => lastOrDefault = items.LastOrDefault());

        lastOrDefault.Should().Be(0);

        source.Add(42);
        lastOrDefault.Should().Be(42);

        source.Remove(42);
        lastOrDefault.Should().Be(0);
    }

    [Fact]
    public void LastChangedTest()
    {
        Selectable<object> selected = null;

        var first = new Selectable<object>();
        var second = new Selectable<object>();
        var third = new Selectable<object>();

        var source = Source.From([first, third], x => x.Key);

        using var autoSelector = source.Connect()
            .AutoRefresh(x => x.IsSelected)
            .Flatten()
            .Select(change => change.Current)
            .Where(x => x.IsSelected)
            .Subscribe(latest => selected = latest);

        selected.Should().BeNull();

        first.IsSelected = true;
        selected.Should().Be(first);

        third.IsSelected = true;
        selected.Should().Be(third);

        source.AddOrUpdate(second);
        selected.Should().Be(third);

        second.IsSelected = true;
        selected.Should().Be(second);
    }

    [Fact]
    public void LastChangedOrDefaultTest()
    {
        Selectable<object> selected = null;

        var first = new Selectable<object>();
        var second = new Selectable<object> { IsSelected = true };
        var source = new SourceCache<Selectable<object>, Guid>(x => x.Key);

        using var autoSelector = source.Connect()
            .AutoRefresh(x => x.IsSelected)
            .ToCollection()
            .Subscribe(items => selected = items.LastOrDefault(x => x.IsSelected));

        source.AddOrUpdate(first);
        selected.Should().BeNull();

        first.IsSelected = true;
        selected.Should().Be(first);

        source.AddOrUpdate(second);
        selected.Should().Be(second);

        second.IsSelected = false;
        selected.Should().Be(first);

        first.IsSelected = false;
        selected.Should().BeNull();
    }
}