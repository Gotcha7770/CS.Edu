using CS.Edu.Core.MathExt;
using System.Linq;
using System.Reactive.Linq;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class FibonacciTest
{
    [Fact]
    public void Test()
    {
        int tenth1 = Fibonacci.Recursive(10);
        int tenth2 = Fibonacci.Iterator().ElementAt(9);
        int tenth3 = EnumerableExtensions.Generate((X: 0, Y: 1), t => (t.Y, t.X + t.Y)).ElementAt(9).X;
        int tenth4 = Fibonacci.Observable(10).Last();

        tenth1.Should().Be(34);
        tenth2.Should().Be(34);
        tenth3.Should().Be(34);
        tenth4.Should().Be(34);
    }
}