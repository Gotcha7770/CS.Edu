using System;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class SkipWhileTests
{
    private readonly Relation<int> _lessThan = (x, y) => x < y;

    [Fact]
    public void SkipWhile_PrevLessThenNext_SkipFirst2Elements()
    {
        var items = new[] { 1, 2, 3, 2, 1 };
        var result = items.SkipWhile(_lessThan);

        result.Should().BeEquivalentTo(new[] { 2, 1 });
    }

    [Fact]
    public void SkipWhile_OneElement_ReturnsEmpty()
    {
        var result = EnumerableEx.Return(1).SkipWhile(_lessThan);

       result.Should().BeEmpty();
    }

    [Fact]
    public void SkipWhile_Empty_ReturnsEmpty()
    {
        var result = Array.Empty<int>().SkipWhile(_lessThan);

        result.Should().BeEmpty();
    }
}