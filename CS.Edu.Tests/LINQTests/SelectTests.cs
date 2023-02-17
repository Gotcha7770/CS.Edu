using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class SelectTests
{
    [Fact]
    public void SelectManyTest()
    {
        //SelectMany is just: source.Select(selector).Concat()

        IEnumerable<IEnumerable<int>> source = new []
        {
            new []{0, 1, 2},
            new []{3, 4, 5}
        };

        Func<IEnumerable<int>, IEnumerable<string>> selector = x => x.Select(i => i.ToString());

        source.SelectMany(selector).Should()
            .BeEquivalentTo(source.Select(selector).Concat());
    }

    [Fact]
    public void SelectManyTest2()
    {
        var result1 = from x in Enumerable.Range(0, 2)
                      from y in Enumerable.Range(0, 2)
                      from z in Enumerable.Range(0, 2)
                      select x + y + z;

        var result2 = Enumerable.Range(0, 2)
            .SelectMany(x => Enumerable.Range(0, 2), (x, y) => new { x, y })
            .SelectMany(t => Enumerable.Range(0, 2), (t, z) => t.x + t.y + z);
    }
}