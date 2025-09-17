using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.TestCases;
using CS.Edu.Tests.Utils.Models;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class CollectionExtTests
{
    [Theory]
    [ClassData(typeof(InvalidateTestCases))]
    public void Invalidate(IList<KeyValue> source, IEnumerable<KeyValue> patch, IEnumerable<KeyValue> expected)
    {
        source.Invalidate(patch, MergeItems, x => x.Key);

        source.Should()
            .BeEquivalentTo(expected);
    }

    [Theory]
    [ClassData(typeof(AddOrUpdateTestCases))]
    public void AddOrUpdate(IList<KeyValue> source, KeyValue item, IEnumerable<KeyValue> expected)
    {
        source.AddOrUpdate(item, MergeItems);

        source.Should()
            .BeEquivalentTo(expected);
    }

    [Theory]
    [ClassData(typeof(InvalidateTestCases))]
    public void Merge(IList<KeyValue> source, IEnumerable<KeyValue> patch, IEnumerable<KeyValue> expected)
    {
        source.Merge(patch, MergeItems, x => x.Key)
            .Should()
            .BeEquivalentTo(expected);
    }

    private static KeyValue MergeItems(KeyValue x, KeyValue y) => x with { Value = $"{x.Value}/{y.Value}" };
}