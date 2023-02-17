using System;
using System.Linq;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.LINQTests;

public class ToDictionaryTest
{
    private record KeyValue(int Key, string Value);

    private readonly KeyValue[] _items =
    {
        new KeyValue(0, "first"),
        new KeyValue(1, "first"),
        new KeyValue(0, "second"),
        new KeyValue(2, "first"),
        new KeyValue(0, "third"),
        new KeyValue(1, "second")
    };

    [Fact]
    public void ToDictionaryWithGroupBy()
    {
        var dic = _items.GroupBy(x => x.Key)
            .Select(x => x.Last())
            .ToDictionary(x => x.Key, x => x);

        dic.Values.Should().BeEquivalentTo(new[]
        {
            new KeyValue(0, "third"),
            new KeyValue(1, "second"),
            new KeyValue(2, "first")
        });
    }

    [Fact]
    public void ToDictionaryWithoutGroupBy()
    {
        Invoking(() => _items.ToDictionary(x => x.Key, x => x.Value))
            .Should().Throw<ArgumentException>();
    }
}