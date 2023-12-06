using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class EnumerableExtTests
{
    [Fact]
    public void ShrinkDuplicatesTest()
    {
        int[] points = [0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0];

        var result = points.ShrinkDuplicates(1);

        result.Should().BeEquivalentTo([0, 1, 0, 0, 1, 0, 1, 0]);
    }

    [Fact]
    public void ShrinkDuplicatesWithKeySelectorTest()
    {
        var points = new[]
        {
            (X: 0, Y: -9999),
            (X: 5, Y: -9999),
            (X: 10, Y: 0),
            (X: 20, Y: 1),
            (X: 30, Y: -9999),
            (X: 40, Y: 4),
            (X: 50, Y: -9999),
            (X: 60, Y: -9999),
            (X: 70, Y: -9999),
            (X: 80, Y: -9999),
            (X: 90, Y: 0),
            (X: 100, Y: 0)
        };

        var standard = new[]
        {
            (X: 0, Y: -9999),
            (X: 10, Y: 0),
            (X: 20, Y: 1),
            (X: 30, Y: -9999),
            (X: 40, Y: 4),
            (X: 50, Y: -9999),
            (X: 90, Y: 0),
            (X: 100, Y: 0)
        };

        var result = points.ShrinkDuplicates(x => x.Y, -9999);

        result.Should().BeEquivalentTo(standard);
    }
}