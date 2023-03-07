using System;
using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class OnLoadedTests
{
    [Fact]
    public void OnLoadedEmptySource()
    {
        var cache = new SourceCache<Identity<Guid>, Guid>(x => x.Key);
        var list = new List<IChangeSet<Identity<Guid>, Guid>>();
        using var aggregator = cache.Connect()
            .OnLoaded(x => list.Add(x))
            .AsAggregator();

        aggregator.Messages.Should().BeEmpty();
        list.Should().BeEmpty();
    }

    [Fact]
    public void OnLoadedWithInitialValues()
    {
        var cache = Source.From(new []
        {
            new Identity<Guid>(Guid.NewGuid()),
            new Identity<Guid>(Guid.NewGuid())
        }, x => x.Key);

        var list = new List<IChangeSet<Identity<Guid>, Guid>>();
        using var aggregator = cache.Connect()
            .OnLoaded(x => list.Add(x))
            .AsAggregator();

        aggregator.Messages.Should().ContainSingle();
        aggregator.Messages.Should().OnlyContain(x => x.Adds == 2);
        list.Should().ContainSingle();
        list.Should().OnlyContain(x => x.Adds == 2);
    }

    [Fact]
    public void OnLoadedEmptySource_IgnoreUpdates()
    {
        var cache = new SourceCache<Identity<Guid>, Guid>(x => x.Key);
        var list = new List<IChangeSet<Identity<Guid>, Guid>>();
        using var aggregator = cache.Connect()
            .OnLoaded(x => list.Add(x))
            .AsAggregator();

        cache.AddOrUpdate(new Identity<Guid>(Guid.NewGuid()));

        aggregator.Messages.Should().ContainSingle();
        aggregator.Messages.Should().OnlyContain(x => x.Adds == 1);
        list.Should().BeEmpty();
    }

    [Fact]
    public void OnLoadedWithInitialValues_IgnoreUpdates()
    {
        var cache = Source.From(new[]
        {
            new Identity<Guid>(Guid.NewGuid()),
            new Identity<Guid>(Guid.NewGuid())
        }, x => x.Key);

        var list = new List<IChangeSet<Identity<Guid>, Guid>>();
        using var aggregator = cache.Connect()
            .OnLoaded(x => list.Add(x))
            .AsAggregator();

        cache.AddOrUpdate(new Identity<Guid>(Guid.NewGuid()));

        aggregator.Messages.Should().HaveCount(2);
        aggregator.Messages[0].Adds.Should().Be(2);
        aggregator.Messages[1].Adds.Should().Be(1);
        list.Should().ContainSingle();
        list.Should().OnlyContain(x => x.Adds == 2);
    }
}