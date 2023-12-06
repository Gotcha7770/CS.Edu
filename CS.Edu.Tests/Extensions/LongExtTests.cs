using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class LongExtTests
{
    [Fact]
    public void LongRange_Empty()
    {
        IEnumerable<long> items = Numbers.Range(0, 0);

        items.Should().BeEmpty();
    }

    [Fact]
    public void LongRange_Single()
    {
        IEnumerable<long> items = Numbers.Range(0, 1);

        items.Should().BeEquivalentTo([0L]);
    }

    [Fact]
    public void LongRange_Several()
    {
        IEnumerable<long> items = Numbers.Range(0, 10);

        items.Should().BeEquivalentTo([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
    }

    [Fact]
    public void LongRange_SeveralWithStep()
    {
        IEnumerable<long> items = Numbers.Range(0, 10, 10);

        items.Should().BeEquivalentTo([0, 10, 20, 30, 40, 50, 60, 70, 80, 90]);
    }

    [Fact]
    public void LongRange_OutOfRange()
    {
        long[] items = Numbers.Range(long.MaxValue - 1, 1).ToArray();

        items.First().Should().Be(long.MaxValue - 1);
        items.Last().Should().Be(long.MaxValue - 1);
    }
}