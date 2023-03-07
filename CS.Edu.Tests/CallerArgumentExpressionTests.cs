using System;
using System.Runtime.CompilerServices;
using CS.Edu.Tests.Utils;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests;

public class CallerArgumentExpressionTests
{
    private static void Check(bool condition, [CallerArgumentExpression("condition")] string message = default)
    {
        if (!condition)
            throw new Exception($"Condition failed: {message}");
    }

    [Fact]
    public void ExceptionMessageFromArgument()
    {
        Identity<int> identity = null;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        Invoking(() => Check(identity is { Key: 3 }))
            .Should().Throw<Exception>()
            .WithMessage("Condition failed: identity is { Key: 3 }");
    }
}