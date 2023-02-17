using System;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class FunctionsTests
{
    [Fact]
    public void ApplyPartial()
    {
        Func<int, int, int> func = (a, b) => a + b;
        var partial = func.ApplyPartial(12);

        partial.Should().BeOfType<Func<int, int>>();
        partial(12).Should().Be(24);
    }

    [Fact]
    public void Curry()
    {
        Func<int, int, int, int[]> func = (a, b, c) => new[] { a, b, c };
        var curried = func.Curry();

        curried.Should().BeOfType<Func<int, Func<int, Func<int, int[]>>>>();
        curried(12)(13)(14).Should().BeEquivalentTo(new [] { 12, 13, 14 });
    }
}