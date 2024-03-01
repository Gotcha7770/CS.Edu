using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ExceptTests
{
    [Fact]
    public void SourceIsNull_ThrowsArgumentNullException()
    {
        IEnumerable<int> none = null;
        Invoking(() => none.Except(0)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SourceIsEmpty_ReturnsEmpty()
    {
        var items = Enumerable.Empty<int>();
        items.Except(0).Should().BeEmpty();
    }

    [Fact]
    public void Except_OnlyTargetItem_ReturnsEmpty()
    {
        var items = EnumerableEx.Return(0);
        items.Except(0).Should().BeEmpty();
    }

    [Fact]
    public void Except_EnumerableWithTargetItem_ReturnsWithoutTargetItem()
    {
        var items = Enumerable.Range(0, 3);
        var result = items.Except(1);

        result.Should().BeEquivalentTo([0, 2]);
    }

    [Fact]
    public void Except_EnumerableWithoutTargetItem_ReturnsSource()
    {
        var items = Enumerable.Range(0, 5);
        var result = items.Except(10);

        result.Should().BeEquivalentTo([0, 1, 2, 3, 4]);
    }
}