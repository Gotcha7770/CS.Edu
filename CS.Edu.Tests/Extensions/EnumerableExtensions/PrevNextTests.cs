using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Iterators;
using DynamicData.Kernel;
using FluentAssertions;
using Xunit;
using EnumerableEx = System.Linq.EnumerableEx;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class PrevNextTests
{
    private readonly IEnumerable<int> _items = Enumerable.Range(0, 5);

    [Fact]
    public void EmptySource_ReturnsEmptyIterator()
    {
        var items = Enumerable.Empty<int>().ToPrevNextIterator();

        items.Should().BeEmpty();
    }

    [Fact]
    public void SourceWithSingleValue_ReturnsJustCurrent()
    {
        var items = EnumerableEx.Return(0).ToPrevNextIterator();

        items.Should()
            .BeEquivalentTo([new PrevNextValue<int>(Optional<int>.None, 0, Optional<int>.None)]);
    }

    [Fact]
    public void SourceWithManyValues_ReturnsFirstItemWithNextValue()
    {
        var first = _items.ToPrevNextIterator().First();

        first.Should()
            .BeEquivalentTo(new PrevNextValue<int>(Optional<int>.None, 0, 1));
    }

    [Fact]
    public void MiddleItems_AllItemsHasPreviousAndNextValues()
    {
        var item = _items.ToPrevNextIterator().Skip(1).Take(3);

        item.Should()
            .BeEquivalentTo(new[]
            {
                new PrevNextValue<int>(0, 1, 2),
                new PrevNextValue<int>(1, 2, 3),
                new PrevNextValue<int>(2, 3, 4)
            });
    }

    [Fact]
    public void LastItemWithNoneNextValue()
    {
        var last = _items.ToPrevNextIterator().Last();

        last.Should()
            .BeEquivalentTo(new PrevNextValue<int>(3, 4, Optional<int>.None));
    }
}