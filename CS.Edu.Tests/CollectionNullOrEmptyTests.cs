using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests;

[TestFixture]
public class CollectionNullOrEmptyTests
{
    [TestCase(null, ExpectedResult = true)]
    [TestCase(new int[0], ExpectedResult = true)]
    [TestCase(new[] { 1 }, ExpectedResult = false)]
    public bool EnumerableIsNullOrEmpty(int[] input)
    {
        return input is null || input.IsEmpty();

        //return enumerable is null or [];
        //return enumerable is null or empty;
        //return enumerable is not (null or empty);
    }

    [TestCase(null, ExpectedResult = false)]
    [TestCase(new int[0], ExpectedResult = false)]
    [TestCase(new[] { 1 }, ExpectedResult = true)]
    public bool EnumerableIsNotNullOrEmpty(int[] input)
    {
        return input is not null && input.Any();
    }

    [TestCase(null, ExpectedResult = true)]
    [TestCase(new int[0], ExpectedResult = true)]
    [TestCase(new[] { 1 }, ExpectedResult = false)]
    public bool ListIsNullOrEmpty(int[] input)
    {
        return input?.ToList() is null or { Count: 0 };
    }

    [TestCase(null, ExpectedResult = false)]
    [TestCase(new int[0], ExpectedResult = false)]
    [TestCase(new[] { 1 }, ExpectedResult = true)]
    public bool ListIsNotNullOrEmpty(int[] input)
    {
        return input?.ToList() is { Count: > 0 };
    }
}