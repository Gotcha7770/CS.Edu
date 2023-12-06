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

        bool[] items = [true];
        func(items).Should().BeTrue();

        items = [false];
        func(items).Should().BeFalse();

        items = [true, false, true];
        func(items).Should().BeNull();

        items = [false, true, false];
        func(items).Should().BeNull();

        items = [false, false, false];
        func(items).Should().BeFalse();

        items = [true, true, true];
        func(items).Should().BeTrue();
    }

    [Fact]
    public void EnumerableOfNullableBooleansTest()
    {
        var func = new Func<IEnumerable<bool?>, bool?>(source =>
        {
            return source.Aggregate((acc, cur) => acc.HasValue && acc.Value == cur ? cur : null);
        });

        bool?[] items = [true];
        func(items).Should().BeTrue();

        items = [false];
        func(items).Should().BeFalse();

        items = [true, false, true];
        func(items).Should().BeNull();

        items = [false, false, false];
        func(items).Should().BeFalse();

        items = [true, true, true];
        func(items).Should().BeTrue();

        items = [true, true, null];
        func(items).Should().BeNull();
    }
}