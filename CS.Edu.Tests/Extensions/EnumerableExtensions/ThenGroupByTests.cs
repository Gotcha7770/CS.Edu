using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ThenGroupByTests
{
    private record Person(string FirstName, string LastName, string Department);
    private readonly IEnumerable<Person> _empty = Enumerable.Empty<Person>();

    private readonly Person[] _source =
    {
        new Person("Bob", "Wilkins", "DevOps"),
        new Person("Bob", "Farnsworth", "DevOps"),
        new Person("Alice", "Wilkins", "HR"),
        new Person("Frank", "Zummer", "IT"),
        new Person("John", "Snow", "Dev"),
        new Person("Jack", "Daniels", "IT"),
        new Person("Bill", "Murrey", "Dev"),
        new Person("Alice", "Milano", "Dev"),
        new Person("Bred", "Pit", "HR"),
        new Person("Anny", "Hall", "DevOps"),
        new Person("Alice", "White", "Dev"),
    };

    [Fact]
    public void EmptySource_ReturnsEmptyEnumerable()
    {
        var result = _empty.GroupBy(x => x.FirstName).ThenBy(x => x.LastName);
        result.Should().BeEmpty();
    }

    [Fact]
    public void GroupOnSecondLayer()
    {
        var result = _source.GroupBy(x => x.LastName[0]).ThenBy(x => x.FirstName[0]);
    }

    [Fact]
    public void GroupOnThirdLayer()
    {
        var result = _source.GroupBy(x => x.Department)
            .ThenBy(x => x.FirstName)
            .ThenBy(x => x.LastName);
    }
}