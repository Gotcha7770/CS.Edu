using Bogus;

namespace CS.Edu.Tests.Utils.Models;

public static class Fakes
{
    public static Faker<Department> DepartmentFaker { get; } = new Faker<Department>()
        .CustomInstantiator(f => new Department(f.Company.CompanyName(), f.Address.City()));

    public static Faker<Employee> EmployeeFaker { get; } = new Faker<Employee>()
        .CustomInstantiator(f => new Employee(f.Person.FirstName, string.Empty));
}