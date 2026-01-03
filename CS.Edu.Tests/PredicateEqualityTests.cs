using System;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class PredicateEqualityTests
{
    [Fact]
    public void TwoPredicatesWithAnonymousMethods_NotEquals()
    {
        var predicate1 = new Predicate<long>(x => x.IsPrime());
        var predicate2 = new Predicate<long>(x => x.IsPrime());

        predicate1.Should().NotBe(predicate2);
    }

    [Fact]
    public void TwoPredicatesWithMethods_Equals()
    {
        var predicate1 = new Predicate<long>(Numbers.IsPrime);
        var predicate2 = new Predicate<long>(Numbers.IsPrime);

        predicate1.Should().Be(predicate2);
        predicate1.Should().NotBeSameAs(predicate2);
    }

    [Fact]
    public void TwoIdentityFunc_NotEquals()
    {
        var func1 = new Func<int, int>(x => x);
        var func2 = new Func<int, int>(x => x);

        func1.Should().NotBe(func2);
    }

    [Fact]
    public void TwoIdentityFunc_Equals()
    {
        var func1 = Func<int, int>.Identity;
        var func2 = Func<int, int>.Identity;

        func1.Should().Be(func2);
        func1.Should().BeSameAs(func2);
    }

    [Fact]
    public void TwoEmptyActions_NotEquals()
    {
        Action<int> act1 = x => { };
        Action<int> act2 = x => { };

        act1.Should().NotBe(act2);
    }

    [Fact]
    public void TwoEmptyActions_Equals()
    {
        var act1 = Action<int>.Idle;
        var act2 = Action<int>.Idle;

        act1.Should().Be(act2);
        act1.Should().BeSameAs(act2);
    }
}