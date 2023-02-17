using CS.Edu.Core.MathExt;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class FactorialTest
{
    [Fact]
    public void Test()
    {
        int fact1 = Factorial.Recursive(5);
        int fact2 = Factorial.Iterator().ElementAt(5);
        int fact3 = Enumerable.Range(0, 6).Aggregate((acc, cur) => acc == 0 ? 1 : acc * cur);

        fact1.Should().Be(120);
        fact2.Should().Be(120);
        fact3.Should().Be(120);
    }
}