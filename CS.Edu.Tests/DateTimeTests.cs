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

    [Theory]
    [ClassData(typeof(DateTimeKindEqualityCases))]
    public void IsKindAffectedEquality(DateTime one, DateTime other, bool expected)
    {
        one.Equals(other)
            .Should()
            .Be(expected);
    }

    [Theory]
    [ClassData(typeof(DateTimeKindComparisonCases))]
    public void IsKindAffectedComparison(DateTime one, DateTime other, int expected)
    {
        one.CompareTo(other)
            .Should()
            .Be(expected);
    }

    public class DateTimeKindEqualityCases : TheoryData<DateTime, DateTime, bool>
    {
        public DateTimeKindEqualityCases()
        {
            Add(new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Unspecified),
                new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Local),
                true);

            Add(new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Unspecified),
                new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                true);

            Add(new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Local),
                true);
        }
    }

    public class DateTimeKindComparisonCases : TheoryData<DateTime, DateTime, int>
    {
        public DateTimeKindComparisonCases()
        {
            Add(new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Unspecified),
                new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Local),
                0);

            Add(new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Unspecified),
                new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                0);

            Add(new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2012, 12, 12, 0, 0, 0, DateTimeKind.Local),
                0);
        }
    }
}