using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions;

public class CollectionExtTests
{
    private record KeyValue(int Key, string Value);

    [Fact]
    public void InvalidateCollection()
    {
        var source = new List<KeyValue>()
        {
            new KeyValue(0, "Zero"),
            new KeyValue(1, "One"),
            new KeyValue(2, "Two"),
            new KeyValue(3, "Three"),
            new KeyValue(4, "Four")
        };

        var patch = new[]
        {
            new KeyValue(0, "Changed"),
            new KeyValue(1, "Changed"),
            new KeyValue(5, "Five"),
            new KeyValue(6, "Six")
        };

        Merge<KeyValue> mergeFunc = (x, y) => x with { Value = $"{x.Value}/{y.Value}" };

        source.Invalidate(patch, mergeFunc, x => x.Key);

        source.Should().BeEquivalentTo(new[]
        {
            new KeyValue(0, "Zero/Changed"),
            new KeyValue(1, "One/Changed"),
            patch[2],
            patch[3]
        });
    }
}