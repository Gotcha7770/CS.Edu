using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests;

[TestFixture]
public class AsyncEnumerable
{
    record Customer(Guid Id);

    record Order(Guid Id, Guid Owner);

    private readonly Customer[] _customers =
    {
        new Customer(Guid.NewGuid()),
        new Customer(Guid.NewGuid()),
        new Customer(Guid.NewGuid())
    };

    private async Task<Order[]> GetOrdersTask(Guid customerId)
    {
        var orders = Enumerable.Range(0, 5)
            .Select(_ => new Order(Guid.NewGuid(), customerId))
            .ToArray();

        return await Task.FromResult(orders);
    }

    private IAsyncEnumerable<Order> GetOrdersEnumerable(Guid customerId)
    {
        return Enumerable.Range(0, 5)
            .Select(_ => new Order(Guid.NewGuid(), customerId))
            .ToAsyncEnumerable();
    }

    [Test]
    public void ExampleWithTask()
    {
        var query = _customers.Select(x => GetOrdersTask(x.Id));
    }

    [Test]
    public async Task ExampleWithAsyncEnumerable()
    {
        var query = _customers.ToAsyncEnumerable()
            .SelectMany(x => GetOrdersEnumerable(x.Id));

        await foreach (var order in query)
        {
            //...
        }
    }
}