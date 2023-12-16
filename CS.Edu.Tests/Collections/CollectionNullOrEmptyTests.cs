using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Collections;

public class CollectionNullOrEmptyTests
{
    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[0], true)]
    [InlineData(new[] { 1 }, false)]
    public void EnumerableIsNullOrEmpty(IEnumerable<int> input, bool expected)
    {
        var result = input is null || input.IsEmpty();
        result.Should().Be(expected);

        //return enumerable is null or [];
        //return enumerable is null or empty;
        //return enumerable is not (null or empty);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(new int[0], false)]
    [InlineData(new[] { 1 }, true)]
    public void EnumerableIsNotNullOrEmpty(IEnumerable<int> input, bool expected)
    {
        var result = input is not null && input.Any();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData(new int[0], true)]
    [InlineData(new[] { 1 }, false)]
    public void ListIsNullOrEmpty(int[] input, bool expected)
    {
        var result = input?.ToList() is null or { Count: 0 };
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(new int[0], false)]
    [InlineData(new[] { 1 }, true)]
    public void ListIsNotNullOrEmpty(int[] input, bool expected)
    {
        var result = input?.ToList() is { Count: > 0 };
        result.Should().Be(expected);
    }
}