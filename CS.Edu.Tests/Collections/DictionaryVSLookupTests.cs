using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Tests.Utils;
using CS.Edu.Tests.Utils.Models;
using FluentAssertions;
using Xunit;
using static FluentAssertions.FluentActions;

namespace CS.Edu.Tests.Collections;

public class DictionaryVSLookupTests
{
    private readonly IEnumerable<Item> _source = new[]
    {
        new Item(1, "unit"),
        new Item(2, "first deuce"),
        new Item(2, "second deuce"),
        new Item(3, "three")
    };

    [Fact]
    public void ToDictionary_DuplicateKeys_ThrowsException()
    {
        Invoking(() => _source.ToDictionary(x => x.Key))
            .Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public void ToDictionary_HandleDuplicateKeys()
    {
        // you have to decide which of the elements with the same key to choose
        // for example the first one
        var result = _source.GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x.First());

        result.Values.Should()
            .BeEquivalentTo([
                new Item(1, "unit"),
                new Item(2, "first deuce"),
                new Item(3, "three")
            ]);
    }

    [Fact]
    public void ToLookup()
    {
        _source.ToLookup(x => x.Key)
            .Should()
            .BeEquivalentTo(_source.GroupBy(x => x.Key));
    }
}