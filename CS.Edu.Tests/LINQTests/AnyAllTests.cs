using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class AnyAllTests
{
    [Fact]
    public void All_WithoutItems_ReturnsTrue()
    {
        var result = Enumerable.Empty<int>().All(x => x % 2 == 0);

        result.Should().BeTrue();

        var items = Enumerable.Empty<int>();
        result = items.Any() && items.All(x => x % 2 == 0);

        result.Should().BeFalse();

        result = Enumerable.Empty<int>()
            .Select(x => x % 2 == 0)
            .DefaultIfEmpty(false)
            .All(x => x);

        result.Should().BeFalse();
    }

    [Fact]
    public void Any_WithoutItems_ReturnsFalse()
    {
        var result = Enumerable.Empty<int>().Any(x => x % 2 != 0);

        result.Should().BeFalse();
    }
}