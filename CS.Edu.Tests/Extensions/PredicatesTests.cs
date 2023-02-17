using System;
using CS.Edu.Core.Extensions;
using Shouldly;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class PredicatesTests
{
    [Theory]
    [InlineData(2, false)]
    [InlineData(3, false)]
    [InlineData(6, true)]
    [InlineData(10, false)]
    [InlineData(12, true)]
    public void PredicatesAndTest(int value, bool expected)
    {
        Predicate<int> predicate1 = x => x % 2 == 0;
        Predicate<int> predicate2 = x => x % 3 == 0;
        Predicate<int> predicate3 = predicate1
            .And(predicate2);

        predicate3(value).ShouldBe(expected);
    }

    [Theory]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(5, false)]
    [InlineData(6, true)]
    public void PredicatesOrTest(int value, bool expected)
    {
        Predicate<int> predicate1 = x => x % 2 == 0;
        Predicate<int> predicate2 = x => x % 3 == 0;
        Predicate<int> predicate3 = predicate1
            .Or(predicate2);

        predicate3(value).ShouldBe(expected);
    }
}