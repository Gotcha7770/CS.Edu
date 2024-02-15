using System;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests;

public class DateTimeTests
{
    [Fact]
    public void DealWithOffset()
    {
        DateTime dateTime = new DateTime(2012, 12, 12);
        DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.FromHours(10));
    }

    [Fact]
    public void GetDayOfYearTest()
    {
        var date = new DateOnly(2013, 6, 6);
        date.DayOfYear.Should().Be(157);

        date = new DateOnly(2012, 6, 6);
        date.DayOfYear.Should().Be(158);
    }

    [Fact]
    public void LeapYearTest()
    {
        Invoking(() =>
            {
                var date = new DateTime(2013, 2, 29);
            })
            .Should()
            .Throw<ArgumentOutOfRangeException>();
    }
}