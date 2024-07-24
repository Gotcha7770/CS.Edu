using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils.Models;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class JoinTests
{
    private static readonly IEnumerable<int> Source = Enumerable.Range(0, 50);

    private static readonly Department[] Departments = Fakes.DepartmentFaker.Generate(3)
        .ToArray();

    private static readonly Employee[] Employees =
    [
        Fakes.EmployeeFaker.Generate() with { Department = Departments[0].Name },
        Fakes.EmployeeFaker.Generate() with { Department = Departments[1].Name },
        Fakes.EmployeeFaker.Generate() with { Department = Departments[1].Name },
        Fakes.EmployeeFaker.Generate()
    ];

    private static readonly (int, int)[] Standard =
    [
        (0, 0),
        (1, 1),
        (2, 4),
        (3, 9),
        (4, 16),
        (5, 25),
        (6, 36),
        (7, 49)
    ];

    [Fact]
    public void JoinQuery()
    {
        var query = from x in Source
                    join y in Source on x * x equals y
                    select (x, y);

        query.Should().BeEquivalentTo(Standard);
    }

    [Fact]
    public void WhereQuery()
    {
        var query = from x in Source
                    from y in Source
                    where x * x == y
                    select (x, y);

        query.Should().BeEquivalentTo(Standard);
    }

    [Fact]
    public void WhyShouldIUseJoin()
    {
        var dictionary = Departments.ToDictionary(x => x.Name, x => x.City);

        //inner join
        var result1 = Employees
            .Select(x => dictionary.GetValueOrDefault(x.Department))
            .Where(x => x is not null);

        var result2 = Employees.Join(Departments,
            l => l.Department,
            r => r.Name,
            (_, r) => r.City);

        result1.Should()
            .BeEquivalentTo(result2);

        //left join
        result1 = Employees
            .Select(x => dictionary.GetValueOrDefault(x.Department));

        result2 = Employees.LeftJoin(Departments,
            l => l.Department,
            r => r.Name,
            (_, r) => r?.City);

        result1.Should()
            .BeEquivalentTo(result2);
    }

    [Fact]
    public void FullOuterJoinQuery()
    {
        var result = Employees.FullOuterJoin(
            Departments,
            l => l.Department,
            r => r.Name,
            (l, r, _) => (l, r));
    }

    [Fact]
    public void All_Joins_With_LINQ_Expressions()
    {
        // https://habr.com/ru/articles/504854/

        // LEFT OUTER JOIN
        var result =
            from left in Employees.Outer()
            join right in Departments
                on left.Department equals right.Name
            select (left, right);

        // RIGHT OUTER JOIN
        result =
            from left in Employees.Inner()
            join right in Departments.Outer()
                on left.Department equals right.Name
            select (left, right);

        // FULL OUTER JOIN
        result =
            from left in Employees.Outer()
            join right in Departments.Outer()
                on left.Department equals right.Name
            select (left, right);
    }

    [Fact]
    public async Task CombineSyncAndAsyncInQuery()
    {
        var query = from employee in Employees
                    join department in Departments.ToAsyncEnumerable() on employee.Department equals department.Name
                    select (employee.Name, department.City);

        var result1 = await Employees.Join(Departments.ToAsyncEnumerable(),
                l => l.Department,
                r => r.Name,
                (l, r) => (l.Name, r.City))
            .ToArrayAsync();

        query = from employee in Employees.ToAsyncEnumerable()
                join department in Departments on employee.Department equals department.Name
                select (employee.Name, department.City);

        var result2 = await Employees.ToAsyncEnumerable()
            .Join(Departments,
                l => l.Department,
                r => r.Name,
                (l, r) => (l.Name, r.City))
            .ToArrayAsync();

        result1.Should()
            .BeEquivalentTo(result2);
    }
}