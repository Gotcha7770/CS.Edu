using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.ReactiveTests;

public class RefreshAllTests
{
    private readonly Valuable<string>[] _items = Enumerable.Range(1, 100)
        .Select(i => new Valuable<string>(i.ToString()))
        .ToArray();

    [Fact]
    public void RefreshAllTest()
    {
        using var refresher = new Subject<Unit>();
        using var cache = Source.From(_items, x => x.Key);
        using var aggregator = cache.Connect()
            .AutoRefreshOnObservable(_ => refresher)
            .SkipInitial()
            .AsAggregator();

        refresher.OnNext(Unit.Default);

        aggregator.Messages.Should().HaveCount(100);
        aggregator.Messages.SelectMany(x => x).Should()
            .OnlyContain(x => x.Reason == ChangeReason.Refresh);
    }
}