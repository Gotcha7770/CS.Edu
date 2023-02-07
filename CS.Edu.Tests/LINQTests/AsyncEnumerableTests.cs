using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

// https://www.dotnetcurry.com/csharp/async-streams
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

    private IAsyncEnumerable<Order> GetOrdersAsyncEnumerable(Guid customerId)
    {
        return Enumerable.Range(0, 5)
            .Select(_ => new Order(Guid.NewGuid(), customerId))
            .ToAsyncEnumerable();
    }

    [Fact]
    public void UnwrapProblem()
    {
        // Here we are dealing with a combination of two effects
        IEnumerable<Task<int>> items = null;

        // Select operator treats the function result as a simple type.
        // So you can transform Task element in Task with any other type,
        // you can even transform it in IAsyncEnumerable (because it doesn't get rid of the async effect),
        // But you can`t change the order of effects.
        // Reversing the order of effects would require us to put an await between the variable and the evaluation,
        // but IEnumerable doesn't know how to deal with await key word.
        // Obviously it must be some other type
        var enumeration = items.Select(async x => (await x).ToString());

        // Hoverer, we have two options
        // Transform IEnumerable<Task<T>> to Task<IEnumerable<T>>
        var task = Task.WhenAll(items);

        // or to IAsyncEnumerable<T>
        var asyncEnumeration = items.ToAsyncEnumerable();
    }

    [Fact]
    public async void SkipWithEnumerableOfTasks()
    {
        IEnumerable<Task<Order>> query = Enumerable.Range(0, 1)
            .Select(_ => Task.FromResult(new Order(Guid.NewGuid(), _customers[1].Id)));

        var singleItem = await query.Skip(5).First();

    }

    [Fact]
    public async void PagingWithEnumerableOfTasks()
    {
        IEnumerable<Task<Order[]>> query = _customers.Select(x => GetOrdersTask(x.Id));

        foreach (var task in query)
        {
            var orders = await task;
            // how to paging?
        }
    }

    [Fact]
    public async Task PagingWithAsyncEnumerable()
    {
        var query = _customers.ToAsyncEnumerable()
            .SelectMany(x => GetOrdersAsyncEnumerable(x.Id));

        await foreach (var order in query)
        {
            // we can get exactly as much as we want
        }
    }
}