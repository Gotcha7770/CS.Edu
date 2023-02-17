using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class IntExtTests
{
    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, false)]
    [InlineData(10, true)]
    [InlineData(11, false)]
    [InlineData(99, false)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public void IsEvenTest(int input, bool expected)
    {
        input.IsEven().Should().Be(expected);
    }

    [Theory]
    [InlineData(0, new long[0])]
    [InlineData(1, new[] { 1L })]
    [InlineData(2, new[] { 1L, 2 })]
    [InlineData(3, new[] { 1L, 3 })]
    [InlineData(4, new[] { 1L, 2, 2 })]
    [InlineData(6, new[] { 1L, 2, 3 })]
    [InlineData(12, new[] { 1L, 2, 2, 3 })]
    [InlineData(120, new[] { 1L, 2, 2, 2, 3, 5 })]
    public void FactorizeTest(long input, long[] expected)
    {
        input.Factorize().Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, false)]
    [InlineData(5, true)]
    [InlineData(6, false)]
    [InlineData(7, true)]
    [InlineData(11, true)]
    [InlineData(13, true)]
    [InlineData(17, true)]
    [InlineData(19, true)]
    [InlineData(21, false)]
    [InlineData(23, true)]
    [InlineData(101, true)]
    [InlineData(1009, true)]
    [InlineData(10007, true)]
    public void IsPrimeTest(long input, bool expected)
    {
        input.IsPrime().Should().Be(expected);
    }
}