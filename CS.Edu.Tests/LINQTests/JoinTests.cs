using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class JoinTests
{
    private static readonly IEnumerable<int> Source = Enumerable.Range(0, 50);

    private static readonly (int, int)[] Standard =
    {
        (0, 0),
        (1, 1),
        (2, 4),
        (3, 9),
        (4, 16),
        (5, 25),
        (6, 36),
        (7, 49)
    };

    [Fact]
    public void JoinQuery()
    {
        var query = from x in Source
                    join y in Source on x * x equals y
                    select (x, y);

        query.Should().BeEquivalentTo(Standard);
    }

    [Fact]
    public void WhereQuery()
    {
        var query = from x in Source
                    from y in Source
                    where x * x == y
                    select (x, y);

        query.Should().BeEquivalentTo(Standard);
    }
}