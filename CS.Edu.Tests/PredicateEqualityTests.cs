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
        var func1 = Functions.Identity<int>();
        var func2 = Functions.Identity<int>();

        func1.Should().Be(func2);
    }

    [Fact]
    public void TwoEmptyActions_NotEquals()
    {
        var act1 = new Action<int>(x => {});
        var act2 = new Action<int>(x => {});

        act1.Should().NotBe(act2);
    }

    [Fact]
    public void TwoEmptyActions_Equals()
    {
        var act1 = Actions.Empty<int>();
        var act2 = Actions.Empty<int>();

        act1.Should().Be(act2);
    }
}