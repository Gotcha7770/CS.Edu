using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class TakeWhileTests
{
    private readonly Relation<int> _lessThan = (x, y) => x < y;

    [Fact]
    public void TakeWhile_PrevLessThenNext_ReturnsFirst3Elements()
    {
        var items = new[] { 1, 2, 3, 2, 1 };
        var result = items.TakeWhile(_lessThan);

        result.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }

    [Fact]
    public void TakeWhile_OneElement_ReturnsThatElement()
    {
        var result = EnumerableEx.Return(1).TakeWhile(_lessThan);

        result.Should().BeEquivalentTo(new[] { 1 });
    }

    [Fact]
    public void TakeWhile_Empty_ReturnsEmpty()
    {
        var result = Enumerable.Empty<int>().TakeWhile(_lessThan);

        result.Should().BeEmpty();
    }
}