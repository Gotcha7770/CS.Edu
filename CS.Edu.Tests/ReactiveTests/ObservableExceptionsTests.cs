using System;
using System.Reactive.Linq;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.ReactiveTests;

public class ObservableExceptionsTests
{
    [Fact]
    public void ThrowExceptionTest_WithoutOnError()
    {
        Invoking(() => Observable.Throw<Exception>(new Exception()).Subscribe())
            .Should().Throw<Exception>();
    }

    [Fact]
    public void ThrowExceptionTest_WithOnError()
    {
        object result = null;
        Exception exception = null;

        Invoking(() => Observable.Throw<Exception>(new Exception())
                .Subscribe(x => result = x, ex => exception = ex))
            .Should().NotThrow<Exception>();

        result.Should().BeNull();
        exception.Should().NotBeNull();
    }
}