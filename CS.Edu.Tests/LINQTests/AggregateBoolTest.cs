using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class AggregateBoolTest
{
    [Fact]
    public void EnumerableOfBooleansTest()
    {
        var func = new Func<IEnumerable<bool>, bool?>(source =>
        {
            return source.Skip(1).Aggregate<bool, bool?>(source.First(), (acc, cur) => acc == cur ? cur : null);
        });

        var items = new [] { true };
        func(items).Should().BeTrue();

        items = new [] { false };
        func(items).Should().BeFalse();

        items = new [] { true, false, true };
        func(items).Should().BeNull();

        items = new [] { false, true, false };
        func(items).Should().BeNull();

        items = new [] { false, false, false };
        func(items).Should().BeFalse();

        items = new [] { true, true, true };
        func(items).Should().BeTrue();
    }

    [Fact]
    public void EnumerableOfNullableBooleansTest()
    {
        var func = new Func<IEnumerable<bool?>, bool?>(source =>
        {
            return source.Aggregate((acc, cur) => acc.HasValue && acc.Value == cur ? cur : null);
        });

        var items = new bool?[] { true };
        func(items).Should().BeTrue();

        items = new bool?[] { false };
        func(items).Should().BeFalse();

        items = new bool?[] { true, false, true };
        func(items).Should().BeNull();

        items = new bool?[] { false, false, false };
        func(items).Should().BeFalse();

        items = new bool?[] { true, true, true };
        func(items).Should().BeTrue();

        items = new bool?[] { true, true, null };
        func(items).Should().BeNull();
    }
}