using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class TernarySplitTests
{
    private readonly Relation<int, int, int> _isMonotone = (x, y, z) => x < y ? y < z : y > z;

    [Fact]
    public void Split_Empty_ReturnsEmpty()
    {
        var result = Enumerable.Empty<int>().Split(_isMonotone);

        result.Should().BeEmpty();
    }

    [Fact]
    public void Split_OneElement_ReturnsThatElement()
    {
        var result = EnumerableEx.Return(1).Split(_isMonotone);

        result.Should().BeEquivalentTo(new[] { new[] { 1 } });
    }

    [Fact]
    public void Split_TwoElements_ReturnsTheseElements()
    {
        var items = new[] { 1, 2 };
        var result = items.Split(_isMonotone);

        result.Should().BeEquivalentTo(new[] { new[] { 1, 2 } });
    }

    [Fact]
    public void Split_WhilePrevLessThenNext_Returns3Groups()
    {
        var items = new[] { 1, 3, 5, 4, 3, 4, 5 };
        var result = items.Split(_isMonotone);

        result.Should()
            .BeEquivalentTo(new[]
            {
                new[] { 1, 3, 5 },
                new[] { 4, 3 },
                new[] { 4, 5 }
            });
    }

    [Fact]
    public void Split_WhilePrevLessThenNext_Returns3GroupsWithBordersIncluded()
    {
        var items = new[] { 1, 3, 5, 4, 3, 4, 5 };
        var result = items.Split(_isMonotone, SplitOptions.IncludeBorders);

        result.Should()
            .BeEquivalentTo(new[]
            {
                new[] { 1, 3, 5 },
                new[] { 5, 4, 3 },
                new[] { 3, 4, 5 }
            });
    }
}