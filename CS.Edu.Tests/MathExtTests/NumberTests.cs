using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.MathExt;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.MathExtTests;

public class NumberTests
{
    [Theory]
    [InlineData(2)]
    [InlineData(8)]
    [InlineData(10)]
    [InlineData(16)]
    public void DigitsTest_Zero_ReturnsZero(int @base)
    {
        Number.Digits(0, @base).Should()
            .BeEquivalentTo([0]);
    }

    [Fact]
    public void DigitsTest_BaseIs2()
    {
        Number.Digits(0b1011, 2).Should()
            .BeEquivalentTo([1, 1, 0, 1]);
    }

    [Fact]
    public void DigitsTest_BaseIs10()
    {
        Number.Digits(123, 10).Should()
            .BeEquivalentTo([3, 2, 1]);
    }

    [Fact]
    public void ReductionTest()
    {
        int[] array = [3, 2, 1];

        long number = array.Select((x, i) => x * 10.Power(i)).Sum();

        number.Should().Be(123);
    }

    [Fact]
    public void PowerOfTwoTests()
    {
        Number.PowerOfTwo(32).Should()
            .Be((long)Math.Pow(2, 32));
    }
}