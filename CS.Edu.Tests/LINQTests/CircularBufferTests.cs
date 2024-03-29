using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class CircularBufferTests
{
    [Fact]
    public void CircularBufferWithLINQ()
    {
        int[] items = [0, 1, 2];
        var buffer = items.Repeat();

        buffer.Take(5).Should().BeEquivalentTo([0, 1, 2, 0, 1]);
    }
}