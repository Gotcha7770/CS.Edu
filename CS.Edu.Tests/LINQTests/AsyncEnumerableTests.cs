using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class AsyncEnumerableTests
{
    private record Customer(Guid Id);

    private record Order(Guid Id, Guid Owner);

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

    [Fact]
    public async void ExampleWithTask()
    {
        IEnumerable<Task<Order[]>> query = _customers.Select(x => GetOrdersTask(x.Id));

        foreach (var task in query)
        {
            var order = await task;
            // ...
        }
    }

    [Fact]
    public async Task ExampleWithAsyncEnumerable()
    {
        var query = _customers.ToAsyncEnumerable()
            .SelectMany(x => GetOrdersEnumerable(x.Id));

        await foreach (var order in query)
        {
            //...
        }
    }

    [Fact]
    public async Task AsyncGroupBy()
    {
        var query = _customers.ToAsyncEnumerable()
            .GroupBy(x => x.Id);
    }
}