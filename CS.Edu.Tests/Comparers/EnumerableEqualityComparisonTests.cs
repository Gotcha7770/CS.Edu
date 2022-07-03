using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Comparers;
using FluentAssertions;
using NUnit.Framework;
using Shouldly;
using Xunit;

namespace CS.Edu.Tests.Comparers;

public class EnumerableEqualityComparisonTests
{
    [Fact]
    public void BothEnumerableAreNull()
    {
        IEnumerable<int> one = null;

        CollectionAssert.AreEqual(null, null);
        one.Should().Equal(null);
        one.ShouldBe(null);
        FluentActions.Invoking(() => one.SequenceEqual(null)).Should().Throw<ArgumentNullException>();
        EnumerableEqualityComparer<int>.Instance.Equals(null, null).Should().BeTrue();
        StructuralComparisons.StructuralEqualityComparer.Equals(null, null).Should().BeTrue();
    }

    [Fact]
    public void OneOfEnumerableIsNull()
    {
        var one = Enumerable.Range(0, 3);

        CollectionAssert.AreNotEqual(one, null);
        //one.Should().NotEqual(null); /doesn't handle null as argument
        one.ShouldNotBe(null);
        FluentActions.Invoking(() => one.SequenceEqual(null)).Should().Throw<ArgumentNullException>();
        EnumerableEqualityComparer<int>.Instance.Equals(one, null).Should().BeFalse();
        StructuralComparisons.StructuralEqualityComparer.Equals(one, null).Should().BeFalse();
    }

    [Fact]
    public void TwoEqualEnumerables_ReturnsTrue()
    {
        var one = Enumerable.Range(0, 3);
        var other = Enumerable.Range(0, 3);

        CollectionAssert.AreEqual(one, other);
        one.Should().Equal(other);
        one.ShouldBe(other);
        one.SequenceEqual(other).Should().BeTrue();
        EnumerableEqualityComparer<int>.Instance.Equals(one, other).Should().BeTrue();
        //StructuralComparisons.StructuralEqualityComparer.Equals(one, other).Should().BeTrue(); //doesn't handle IEnumerable<T>
    }

    [Fact]
    public void TwoNotEqualEnumerables_ReturnsFalse()
    {
        var one = Enumerable.Range(0, 3);
        var other = Enumerable.Range(0, 2);

        CollectionAssert.AreNotEqual(one, other);
        one.Should().NotEqual(other);
        one.ShouldNotBe(other);
        one.SequenceEqual(other).Should().BeFalse();
        EnumerableEqualityComparer<int>.Instance.Equals(one, other).Should().BeFalse();
        //StructuralComparisons.StructuralEqualityComparer.Equals(one, other).Should().BeFalse(); //doesn't handle IEnumerable<T>
    }
}