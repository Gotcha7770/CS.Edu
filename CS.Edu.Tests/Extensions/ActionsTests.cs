using System;
using AwesomeAssertions;
using Xunit;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.TestCases;

namespace CS.Edu.Tests.Extensions;

public class ActionsTests
{
    [Theory]
    [ClassData(typeof(ActionsTestCases))]
    public void IsIdle(Action action, bool expected)
    {
        action.IsIdle.Should().Be(expected);
    }
}