using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class BinarySplitTests
{
    private readonly Relation<int> _lessThan = (x, y) => x < y;

    [Fact]
    public void Split_Empty_ReturnsEmpty()
    {
        var items = Enumerable.Empty<int>();
        var result = items.Split(_lessThan);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Split_OneElement_ReturnsThatElement()
    {
        int[] items = [1];
        var result = items.Split(_lessThan);

        result.Should().BeEquivalentTo([items]);
    }

    [Fact]
    public void Split_WhilePrevLessThenNext_Returns2Groups()
    {
        int[] items = [1, 2, 3, 2, 3];
        var result = items.Split(_lessThan)
            .ToArray();

        result.Should().BeEquivalentTo(new[] { new[] { 1, 2, 3 }, new[] { 2, 3 } });
    }
}