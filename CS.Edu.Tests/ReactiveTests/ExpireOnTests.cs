using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class ExpireOnTests
{
    private readonly Subject<Unit> _subject = new Subject<Unit>();

    [Fact]
    public void RemoveAllWithFilter()
    {
        var source = Source.From(new[]
        {
            new Valuable<int>(0),
            new Valuable<int>(1)
        }, x => x.Key);

        using (var aggregate = source.Connect()
                   .Filter(_subject.Select(_ => Predicates.False<Valuable<int>>())
                       .StartWith(Predicates.True<Valuable<int>>()))
                   .SkipInitial()
                   .AsAggregator())
        {
            _subject.OnNext(Unit.Default);
            aggregate.Messages.Should().ContainSingle();
            aggregate.Messages.Should().OnlyContain(x => x.Removes == 2);
        }
    }

    [Fact]
    public void ExpireOn_Initial()
    {
        var source = new SourceCache<Valuable<int>, Guid>(x => x.Key);
        using (var aggregate = source.Connect()
                   .ExpireOn(_subject)
                   .AsAggregator())
        {
            source.AddOrUpdate(new Valuable<int>(0));
            source.AddOrUpdate(new Valuable<int>(1));

            aggregate.Messages.Should().HaveCount(2);
            aggregate.Messages.Should().OnlyContain(x => x.Adds == 1);
        }
    }

    [Fact]
    public void ExpireOn_ClearsOnEvaluation()
    {
        var source = Source.From(new[]
        {
            new Valuable<int>(0),
            new Valuable<int>(1)
        }, x => x.Key);

        using (var aggregate = source.Connect()
                   .ExpireOn(_subject)
                   .SkipInitial()
                   .AsAggregator())
        {
            _subject.OnNext(Unit.Default);
            aggregate.Messages.Should().ContainSingle();
            aggregate.Messages.Should().OnlyContain(x => x.Removes == 2);
        }
    }

    [Fact]
    public void ExpireOn_EmptySource_NothingToClear()
    {
        var source = new SourceCache<Valuable<int>, Guid>(x => x.Key);
        using (var aggregate = source.Connect()
                   .ExpireOn(_subject)
                   .AsAggregator())
        {
            _subject.OnNext(Unit.Default);
            aggregate.Messages.Should().BeEmpty();
        }
    }
}