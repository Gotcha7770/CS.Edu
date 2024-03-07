using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class SelectManyTests
{
    [Fact]
    public void SelectManyTest()
    {
        //SelectMany is just: source.Select(selector).Concat()

        int[][] source =
        [
            [0, 1, 2],
            [3, 4, 5]
        ];

        Func<IEnumerable<int>, IEnumerable<string>> selector = x => x.Select(i => i.ToString());

        source.SelectMany(selector)
            .Should()
            .BeEquivalentTo(source.Select(selector)
                .Concat());
    }

    [Fact]
    public void SelectManyTest2()
    {
        var result1 = from x in Enumerable.Range(0, 2)
                      from y in Enumerable.Range(0, 2)
                      from z in Enumerable.Range(0, 2)
                      select x + y + z;

        var result2 = Enumerable.Range(0, 2)
            .SelectMany(_ => Enumerable.Range(0, 2), (x, y) => new { x, y })
            .SelectMany(_ => Enumerable.Range(0, 2), (t, z) => t.x + t.y + z);

        result1.Should()
            .BeEquivalentTo(result2);
    }

    [Fact]
    public async Task CombineSyncAndAsyncInQuery()
    {
        var query = from x in Enumerable.Range(0, 2)
                    from y in AsyncEnumerable.Range(0, 2)
                    from z in Enumerable.Range(0, 2)
                    select x + y + z;

        var result1 = await Enumerable.Range(0, 2)
            .SelectMany(_ => AsyncEnumerable.Range(0, 2), (x, y) => new { x, y })
            .SelectMany(_ => Enumerable.Range(0, 2), (t, z) => t.x + t.y + z)
            .ToArrayAsync();

        query = from x in AsyncEnumerable.Range(0, 2)
                from y in Enumerable.Range(0, 2)
                from z in AsyncEnumerable.Range(0, 2)
                select x + y + z;

        var result2 = await AsyncEnumerable.Range(0, 2)
            .SelectMany(_ => Enumerable.Range(0, 2), (x, y) => new { x, y })
            .SelectMany(_ => AsyncEnumerable.Range(0, 2), (t, z) => t.x + t.y + z)
            .ToArrayAsync();

        result1.Should()
            .BeEquivalentTo(result2);
    }
}