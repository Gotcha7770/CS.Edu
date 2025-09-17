using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions;

public class ThenGroupByTests
{
    private enum Department { HR, IT }

    private record Person(string FirstName, string LastName, Department Department);

    [Fact]
    public void Group_Consuming()
    {
        var people = new List<Person>
        {
            new("John", "Doe", Department.HR),
            new("Jane", "Smith", Department.HR),
            new("Alice", "Johnson", Department.IT),
            new("Bob", "Brown", Department.IT),
            new("Charlie", "Brown", Department.IT),
            new("John", "Black", Department.HR)
        };

        // - HR
        //   - Doe
        //     - John
        //   - Smith
        //     - Jane
        //   - Black
        //     - John
        // - IT
        //   - Johnson
        //     - Alice
        //   - Brown
        //     - Bob
        //     - Charlie

        var usingBuiltInTools = people
            .GroupBy(x => x.Department)
            .ToDictionary(l1 => l1.Key,
                l1 => l1.GroupBy(p => p.LastName).ToDictionary(l2 => l2.Key,
                    l2 => l2.ToLookup(p => p.FirstName)));

        var usingStronglyTypedParameters = people.GroupBy(x => x.Department) // -> Dictionary
            .ThenBy(x => x.LastName) // -> Dictionary
            .ThenBy(x => x.FirstName); // -> Lookup

        var usingNestedGrouping = people.GroupBy(x => x.Department) // -> Dictionary
            .ThenByEx(x => x.LastName) // -> Dictionary
            .ThenByEx(x => x.FirstName); // -> Lookup

        // var itStaff = usingNestedGrouping.GetSubGroup<string>(Department.IT);
        // var browns = itStaff["Brown"];
        // var bob = browns["Bob"].Single();

        // var brownsFromItDepartment =
        //     from itStaff in grouped[Department.IT]
        //     from browns in itStaff["Brown"]
        //     select browns.Match(_ => [], r => r);
        //
        // brownsFromItDepartment
        //     .Should()
        //     .BeEquivalentTo(
        //     [
        //         new Person2("Bob", "Brown", Department.IT),
        //         new Person2("Charlie", "Brown", Department.IT),
        //     ]);

        // var invalidGroup =
        //     from itStaff in grouped["IT"]
        //     from invalid in itStaff["Invalid"]
        //     select invalid;
        //
        // invalidGroup
        //     .Should().Be(Group<string, Person>.Empty);
    }

    [Fact]
    public void EmptySource_ReturnsEmptyEnumerable()
    {
        var tmp = Enumerable.Empty<Person>()
            .GroupBy(x => x.FirstName)
            .ToDictionary(l1 => l1.Key,
                l1 => l1.GroupBy(p => p.LastName).ToDictionary(l2 => l2.Key,
                    l2 => l2.GroupBy(p => p.Department)));

        // Enumerable.Empty<Person>()
        //     .GroupBy(x => x.Department)
        //     .ThenBy(x => x.LastName)
        //     .ThenBy(x => x.FirstName)
        //     .Should()
        //     .BeEmpty();
    }

    // [Fact]
    // public void ThenBy_ShouldReturnSingleGroupWhenNoFurtherGrouping()
    // {
    //     Person[] people =
    //     [
    //         new("John", "Doe", Department.HR),
    //         new("Jane", "Smith", Department.HR),
    //         new("John", "Black", Department.HR)
    //     ];
    //
    //     var grouped = people.GroupBy(x => x.Department)
    //         .ThenBy(x => x.LastName);
    //
    //     grouped.Should()
    //         .SatisfyRespectively(one =>
    //         {
    //             one.Key.Should().Be(Department.HR);
    //             one.Should()
    //                 .ContainSingle()
    //                 .Which
    //                 .Should()
    //                 .BeEquivalentTo(
    //                     new Person[]
    //                         {
    //                             new("John", "Doe", Department.HR),
    //                             new("Jane", "Smith", Department.HR),
    //                             new("John", "Black", Department.HR)
    //                         }
    //                         .ToLookup(x => x.LastName)
    //                 );
    //         });
    // }

    // [Fact]
    // public void ThenBy_ShouldGroupByDepartmentAndThenByLastName()
    // {
    //     var people = new List<Person>
    //     {
    //         new("John", "Doe", Department.HR),
    //         new("Jane", "Smith", Department.HR),
    //         new("Alice", "Johnson", Department.IT),
    //         new("Bob", "Brown", Department.IT),
    //         new("Charlie", "Brown", Department.IT),
    //         new("John", "Black", Department.HR)
    //     };
    //
    //     var grouped = people.GroupBy(p => p.Department)
    //         .ThenBy(p => p.LastName);
    //
    //     grouped.Should()
    //         .SatisfyRespectively(
    //             first =>
    //             {
    //                 first.Key.Should().Be(Department.HR);
    //                 first.Should()
    //                     .ContainSingle()
    //                     .Which
    //                     .Should()
    //                     .BeEquivalentTo(
    //                         new Person[]
    //                             {
    //                                 new("John", "Doe", Department.HR),
    //                                 new("Jane", "Smith", Department.HR),
    //                                 new("John", "Black", Department.HR)
    //                             }
    //                             .ToLookup(x => x.LastName)
    //                     );
    //             },
    //             second =>
    //             {
    //                 second.Key.Should().Be(Department.IT);
    //                 second.Should()
    //                     .ContainSingle()
    //                     .Which
    //                     .Should()
    //                     .BeEquivalentTo(
    //                         new Person[]
    //                             {
    //                                 new("Alice", "Johnson", Department.IT),
    //                                 new("Bob", "Brown", Department.IT),
    //                                 new("Charlie", "Brown", Department.IT)
    //                             }
    //                             .ToLookup(x => x.LastName)
    //                     );
    //             });
    // }

    // [Fact]
    // public void ThenBy_ShouldGroupByDepartmentAndThenByFirstName()
    // {
    //     var people = new List<Person>
    //     {
    //         new("John", "Doe", Department.HR),
    //         new("Jane", "Smith", Department.HR),
    //         new("Alice", "Johnson", Department.IT),
    //         new("Bob", "Brown", Department.IT),
    //         new("Charlie", "Brown", Department.IT),
    //         new("John", "Black", Department.HR)
    //     };
    //
    //     var grouped = people.GroupBy(x => x.Department)
    //         .ThenBy(x => x.FirstName);
    //
    //     grouped.Should()
    //         .SatisfyRespectively(
    //             first =>
    //             {
    //                 first.Key.Should().Be(Department.HR);
    //                 first.Should()
    //                     .ContainSingle()
    //                     .Which
    //                     .Should()
    //                     .BeEquivalentTo(
    //                         new Person[]
    //                             {
    //                                 new("John", "Doe", Department.HR),
    //                                 new("Jane", "Smith", Department.HR),
    //                                 new("John", "Black", Department.HR)
    //                             }
    //                             .ToLookup(x => x.FirstName)
    //                     );
    //             },
    //             second =>
    //             {
    //                 second.Key.Should().Be(Department.IT);
    //                 second.Should()
    //                     .ContainSingle()
    //                     .Which
    //                     .Should()
    //                     .BeEquivalentTo(
    //                         new Person[]
    //                             {
    //                                 new("Alice", "Johnson", Department.IT),
    //                                 new("Bob", "Brown", Department.IT),
    //                                 new("Charlie", "Brown", Department.IT)
    //                             }
    //                             .ToLookup(x => x.FirstName)
    //                     );
    //             });
    // }
}