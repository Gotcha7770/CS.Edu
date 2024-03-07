using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class WhyYouShouldNotUseEmptyListInstance
{
    public static class ListExtensions<T>
    {
        public static readonly List<T> Empty = [];
    }

    [Fact]
    public void SharedEmptyListProblem()
    {
        var one = ListExtensions<int>.Empty;
        one.Add(1);

        var other = ListExtensions<int>.Empty;

        // both lists are equivalent, although they shouldn't be
        one.Should()
            .BeEquivalentTo(other);
    }
}