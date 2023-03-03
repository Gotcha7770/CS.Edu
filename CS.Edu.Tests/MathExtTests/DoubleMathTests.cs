using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.MathExtTests;

public class DoubleMathTests
{
    [Fact]
    public void ProductOfDoubleSpecialValues()
    {
        double.NegativeInfinity.Should().Be(2 * double.NegativeInfinity);
        double.NegativeInfinity.Should().Be(double.NegativeInfinity * 2);
        double.PositiveInfinity.Should().Be(double.NegativeInfinity * double.NegativeInfinity);
        double.PositiveInfinity.Should().Be(double.PositiveInfinity * double.PositiveInfinity);
        double.NaN.Should().Be(double.NaN * double.NegativeInfinity);
        double.NaN.Should().Be(double.NegativeInfinity * double.NaN);
        double.NaN.Should().Be(2 * double.NaN);
        double.NaN.Should().Be(double.NaN * 2);
    }
}