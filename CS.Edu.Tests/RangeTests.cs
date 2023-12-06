using System;
using System.Collections.Generic;
using CS.Edu.Core;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class RangeTests
{
    // https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
    // https://hamidmosalla.com/2017/02/25/xunit-theory-working-with-inlinedata-memberdata-classdata/

    [Theory]
    [MemberData(nameof(RangesForEqualsTests))]
    public void EqualsTest(Range<int> one, Range<int> other, bool expected)
    {
        Equals(one, other).Should().Be(expected);
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(5, true)]
    [InlineData(7, true)]
    [InlineData(9, true)]
    [InlineData(10, false)]
    public void ContainsValueTests(int value, bool expected)
    {
        var range = new Range<int>(5, 9);

        range.Contains(value).Should().Be(expected);
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(5, false)]
    [InlineData(7, true)]
    [InlineData(9, false)]
    [InlineData(10, false)]
    public void ContainsValueExcludeBordersTests(int value, bool expected)
    {
        var range = new Range<int>(5, 9);

        range.Contains(value, RangeParameters.None).Should().Be(expected);
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(5, false)]
    [InlineData(7, true)]
    [InlineData(9, true)]
    [InlineData(10, false)]
    public void ContainsValueExcludeMinTests(int value, bool expected)
    {
        var range = new Range<int>(5, 9);

        range.Contains(value, RangeParameters.IncludeMaximum).Should().Be(expected);
    }

    [Theory]
    [InlineData(4, false)]
    [InlineData(5, true)]
    [InlineData(7, true)]
    [InlineData(9, false)]
    [InlineData(10, false)]
    public void ContainsValueExcludeMaxTests(int value, bool expected)
    {
        var range = new Range<int>(5, 9);

        range.Contains(value, RangeParameters.IncludeMinimum).Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(RangesForContainsTests))]
    public void ContainsRangeTests(Range<int> one, Range<int> other, bool expected)
    {
        one.Contains(other).Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(RangesForIntersectsTests))]
    public void IntersectsTests(Range<int> one, Range<int> other, bool expected)
    {
        one.Intersects(other).Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(RangesForIntersectionTests))]
    public void IntersectionTests(Range<int> one, Range<int> other, Range<int> expected)
    {
        one.Intersection(other).Should().BeEquivalentTo(expected);
    }

    [Theory]
    [MemberData(nameof(RangesForSubtractionTests))]
    public void SubtractionTests(Range<int> one, Range<int> other, Range<int>[] expected)
    {
        one.Subtract(other).Should().BeEquivalentTo(expected);
    }

    [Theory]
    [MemberData(nameof(RangesForSymmetricDifferenceTests))]
    public void SymmetricDifferenceTest(Range<int> one, Range<int> other, Range<int>[] expected)
    {
        Range<int>.SymmetricDifference(one, other).Should().BeEquivalentTo(expected);
        // var one = new Range<int>(0, 6);
        // var other = new Range<int>(4, 10);
        //
        // var result = Range<int>.SymmetricDifference(one, other).ToArray();
    }

    public static IEnumerable<object[]> RangesForEqualsTests
    {
        get
        {
            var standard = new Range<int>(5, 9);

            yield return new object[] { Range<int>.Empty, Range<int>.Empty, true };
            yield return new object[] { standard, null, false };
            yield return new object[] { standard, Range<int>.Empty, false };
            yield return new object[] { standard, new Range<int>(4, 9), false };
            yield return new object[] { standard, new Range<int>(6, 9), false };
            yield return new object[] { standard, new Range<int>(5, 8), false };
            yield return new object[] { standard, new Range<int>(5, 10), false };
            yield return new object[] { standard, new Range<int>(4, 10), false };
            yield return new object[] { standard, new Range<int>(6, 8), false };
            yield return new object[] { standard, new Range<int>(5, 9), true };
            yield return new object[] { standard, standard, true };
        }
    }

    public static IEnumerable<object[]> RangesForContainsTests
    {
        get
        {
            var standard = new Range<int>(5, 9);

            yield return new object[] { standard, new Range<int>(5, 9), true };
            yield return new object[] { standard, new Range<int>(5, 8), true };
            yield return new object[] { standard, new Range<int>(6, 9), true };
            yield return new object[] { standard, new Range<int>(6, 8), true };

            yield return new object[] { standard, new Range<int>(0, 4), false };
            yield return new object[] { standard, new Range<int>(0, 5), false };
            yield return new object[] { standard, new Range<int>(0, 6), false };
            yield return new object[] { standard, new Range<int>(0, 8), false };
            yield return new object[] { standard, new Range<int>(0, 9), false };
            yield return new object[] { standard, new Range<int>(0, 10), false };
            yield return new object[] { standard, new Range<int>(4, 11), false };
            yield return new object[] { standard, new Range<int>(5, 11), false };
            yield return new object[] { standard, new Range<int>(6, 11), false };
            yield return new object[] { standard, new Range<int>(8, 11), false };
            yield return new object[] { standard, new Range<int>(9, 11), false };
            yield return new object[] { standard, new Range<int>(10, 11), false };
        }
    }

    public static IEnumerable<object[]> RangesForIntersectsTests
    {
        get
        {
            var standard = new Range<int>(5, 9);

            yield return new object[] { standard, new Range<int>(0, 5), true };
            yield return new object[] { standard, new Range<int>(0, 6), true };
            yield return new object[] { standard, new Range<int>(0, 8), true };
            yield return new object[] { standard, new Range<int>(0, 9), true };
            yield return new object[] { standard, new Range<int>(0, 10), true };
            yield return new object[] { standard, new Range<int>(4, 8), true };
            yield return new object[] { standard, new Range<int>(4, 9), true };
            yield return new object[] { standard, new Range<int>(4, 10), true };
            yield return new object[] { standard, new Range<int>(5, 8), true };
            yield return new object[] { standard, new Range<int>(5, 9), true };
            yield return new object[] { standard, new Range<int>(5, 10), true };
            yield return new object[] { standard, new Range<int>(6, 8), true };
            yield return new object[] { standard, new Range<int>(6, 9), true };
            yield return new object[] { standard, new Range<int>(6, 10), true };
            yield return new object[] { standard, new Range<int>(8, 9), true };
            yield return new object[] { standard, new Range<int>(8, 10), true };
            yield return new object[] { standard, new Range<int>(9, 10), true };

            yield return new object[] { standard, new Range<int>(0, 4), false };
            yield return new object[] { standard, new Range<int>(10, 15), false };
        }
    }

    public static IEnumerable<object[]> RangesForIntersectionTests
    {
        get
        {
            var standard = new Range<int>(5, 9);

            yield return new object[] { standard, new Range<int>(0, 5), new Range<int>(5, 5) };
            yield return new object[] { standard, new Range<int>(0, 6), new Range<int>(5, 6) };
            yield return new object[] { standard, new Range<int>(0, 8), new Range<int>(5, 8) };
            yield return new object[] { standard, new Range<int>(0, 9), new Range<int>(5, 9) };
            yield return new object[] { standard, new Range<int>(0, 10), new Range<int>(5, 9) };

            yield return new object[] { standard, new Range<int>(4, 8), new Range<int>(5, 8) };
            yield return new object[] { standard, new Range<int>(4, 9), new Range<int>(5, 9) };
            yield return new object[] { standard, new Range<int>(4, 10), new Range<int>(5, 9) };

            yield return new object[] { standard, new Range<int>(5, 8), new Range<int>(5, 8) };
            yield return new object[] { standard, new Range<int>(5, 9), new Range<int>(5, 9) };
            yield return new object[] { standard, new Range<int>(5, 10), new Range<int>(5, 9) };

            yield return new object[] { standard, new Range<int>(6, 8), new Range<int>(6, 8) };
            yield return new object[] { standard, new Range<int>(6, 9), new Range<int>(6, 9) };
            yield return new object[] { standard, new Range<int>(6, 10), new Range<int>(6, 9) };

            yield return new object[] { standard, new Range<int>(8, 9), new Range<int>(8, 9) };
            yield return new object[] { standard, new Range<int>(8, 10), new Range<int>(8, 9) };
            yield return new object[] { standard, new Range<int>(9, 10), new Range<int>(9, 9) };

            yield return new object[] { standard, new Range<int>(0, 4), Range<int>.Empty };
            yield return new object[] { standard, new Range<int>(10, 15), Range<int>.Empty };
        }
    }

    public static IEnumerable<object[]> RangesForSubtractionTests
    {
        get
        {
            var standard = new Range<int>(5, 9);

            yield return [standard, new Range<int>(0, 4), new[] { new Range<int>(5, 9) }];
            yield return [standard, new Range<int>(0, 5), new[] { new Range<int>(5, 9) }];
            yield return [standard, new Range<int>(0, 6), new[] { new Range<int>(6, 9) }];
            yield return [standard, new Range<int>(0, 8), new[] { new Range<int>(8, 9) }];

            yield return [standard, new Range<int>(0, 9), Array.Empty<Range<int>>()];
            yield return [standard, new Range<int>(0, 10), Array.Empty<Range<int>>()];
            yield return [standard, new Range<int>(5, 9), Array.Empty<Range<int>>()];
            yield return [standard, new Range<int>(5, 10), Array.Empty<Range<int>>()];

            yield return [standard, new Range<int>(6, 8), new[] { new Range<int>(5, 6), new Range<int>(8, 9) }];
            yield return [standard, new Range<int>(7, 7), new[] { new Range<int>(5, 7), new Range<int>(7, 9) }];

            yield return [standard, new Range<int>(6, 10), new[] { new Range<int>(5, 6) }];
            yield return [standard, new Range<int>(8, 10), new[] { new Range<int>(5, 8) }];
            yield return [standard, new Range<int>(9, 10), new[] { new Range<int>(5, 9) }];
            yield return [standard, new Range<int>(10, 11), new[] { new Range<int>(5, 9) }];
        }
    }

    public static IEnumerable<object[]> RangesForSymmetricDifferenceTests
    {
        get
        {
            yield return [new Range<int>(0, 4), new Range<int>(5, 9), new[] { new Range<int>(0, 4), new Range<int>(5, 9) }];
            yield return [new Range<int>(0, 5), new Range<int>(5, 9), new[] { new Range<int>(0, 5), new Range<int>(5, 9) }];
            yield return [new Range<int>(0, 6), new Range<int>(5, 9), new[] { new Range<int>(0, 5), new Range<int>(6, 9) }];
            yield return [new Range<int>(5, 9), new Range<int>(0, 6), new[] { new Range<int>(6, 9), new Range<int>(0, 5) }];
            yield return [new Range<int>(0, 10), new Range<int>(3, 7), new[] { new Range<int>(0, 3), new Range<int>(7, 10) }];
            yield return [new Range<int>(3, 7), new Range<int>(0, 10), new[] { new Range<int>(7, 10), new Range<int>(0, 3) }];
        }
    }
}