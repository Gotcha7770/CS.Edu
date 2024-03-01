using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ProductNTests
{
    [Fact]
    public void SourceIsNull_ThrowsArgumentNullException()
    {
        IEnumerable<IEnumerable<int>> none = null;
        Invoking(() => none.ProductN()).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SourceIsEmpty_ReturnsEmpty()
    {
        var items = Enumerable.Empty<IEnumerable<int>>();
        items.ProductN()
            .Should()
            .BeEquivalentTo(EnumerableEx.Return(Enumerable.Empty<int>()));
    }

    [Fact]
    public void TwoCollection_Returns4CollectionsOf2Elements()
    {
        int[][] collections =
        [
            [1, 2],
            [3, 4]
        ];

        collections.ProductN()
            .Should()
            .BeEquivalentTo(new int[][]
            {
                [1, 3],
                [1, 4],
                [2, 3],
                [2, 4]
            });
    }

    [Fact]
    public void ThreeCollections_Returns8CollectionsOf3Elements()
    {
        int[][] collections =
        [
            [1, 2],
            [3, 4],
            [5, 6]
        ];

        collections.ProductN()
            .Should()
            .BeEquivalentTo(new int[][] {
                [1, 3, 5],
                [1, 3, 6],
                [1, 4, 5],
                [1, 4, 6],
                [2, 3, 5],
                [2, 3, 6],
                [2, 4, 5],
                [2, 4, 6]
            });
    }
}