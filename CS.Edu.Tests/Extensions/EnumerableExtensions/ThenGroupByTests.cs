using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Monads;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ThenGroupByTests
{
    private record Person(string FirstName, string LastName, string Department);

    [Fact]
    public void EmptySource_ReturnsEmptyEnumerable()
    {
        Enumerable.Empty<Person>()
            .GroupBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void ThenBy_ShouldReturnSingleGroupWhenNoFurtherGrouping()
    {
        Person[] people =
        [
            new("John", "Doe", "HR"),
            new("Jane", "Smith", "HR"),
            new("Dave", "Black", "HR")
        ];

        var grouped = people.GroupBy(x => x.Department)
            .ThenBy(x => x.LastName);

        grouped
            .Should()
            .BeEquivalentTo(
            [
                new Group<string, Person>(
                    "HR",
                    [
                        new Group<string, Person>("Doe", new Person("John", "Doe", "HR")),
                        new Group<string, Person>("Smith", new Person("Jane", "Smith", "HR")),
                        new Group<string, Person>("Black", new Person("Dave", "Black", "HR"))
                    ])
            ]);
    }

    [Fact]
    public void ThenBy_ShouldGroupByDepartmentAndThenByLastName()
    {
        var people = new List<Person>
        {
            new("John", "Doe", "HR"),
            new("Jane", "Smith", "HR"),
            new("Alice", "Johnson", "IT"),
            new("Bob", "Brown", "IT"),
            new("Charlie", "Brown", "IT"),
            new("John", "Black", "HR")
        };

        var grouped = people.GroupBy(p => p.Department)
            .ThenBy(p => p.LastName);

        grouped.Should()
            .BeEquivalentTo(
            [
                new Group<string, Person>(
                    "HR",
                    [
                        new Group<string, Person>("Doe", new Person("John", "Doe", "HR")),
                        new Group<string, Person>("Smith", new Person("Jane", "Smith", "HR")),
                        new Group<string, Person>("Black", new Person("John", "Black", "HR"))
                    ]),
                new Group<string, Person>(
                    "IT",
                    [
                        new Group<string, Person>("Johnson", new Person("Alice", "Johnson", "IT")),
                        new Group<string, Person>(
                            "Brown",
                            new Person("Bob", "Brown", "IT"),
                            new Person("Charlie", "Brown", "IT"))
                    ])
            ]);
    }

    [Fact]
    public void ThenBy_ShouldGroupByDepartmentAndThenByFirstName()
    {
        var people = new List<Person>
        {
            new("John", "Doe", "HR"),
            new("Jane", "Smith", "HR"),
            new("Alice", "Johnson", "IT"),
            new("Bob", "Brown", "IT"),
            new("Charlie", "Brown", "IT"),
            new("John", "Black", "HR")
        };

        var grouped = people.GroupBy(x => x.Department)
            .ThenBy(x => x.FirstName);
    }

    [Fact]
    public void Group_Consuming()
    {
        var people = new List<Person>
        {
            new("John", "Doe", "HR"),
            new("Jane", "Smith", "HR"),
            new("Alice", "Johnson", "IT"),
            new("Bob", "Brown", "IT"),
            new("Charlie", "Brown", "IT"),
            new("John", "Black", "HR")
        };

        var grouped = people.GroupBy(p => p.Department)
            .ThenBy(p => p.LastName)
            .ToDictionary(x => x.Key);

        var brownsFromItDepartment =
            from itStaff in grouped["IT"]
            from browns in itStaff["Brown"]
            select browns.Match(_ => [], r => r);

        brownsFromItDepartment
            .Should()
            .BeEquivalentTo(
            [
                new Person("Bob", "Brown", "IT"),
                new Person("Charlie", "Brown", "IT"),
            ]);

        var invalidGroup =
            from itStaff in grouped["IT"]
            from invalid in itStaff["Invalid"]
            select invalid;

        invalidGroup
            .Should().Be(Group<string, Person>.Empty);
    }
}