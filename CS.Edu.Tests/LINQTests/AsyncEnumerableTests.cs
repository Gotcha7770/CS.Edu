using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests;

[TestFixture]
public class AsyncEnumerableTests
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
        IEnumerable<Task<Order[]>> query = _customers.Select(x => GetOrdersTask(x.Id));
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

    [Test]
    public async Task PassCancellationToSelector()
    {
        ValueTask<int> AsyncJob(int x, CancellationToken cancellationToken = default) => new ValueTask<int>(x * x);
        ValueTask<IAsyncEnumerable<int>> Unfold(int x, CancellationToken cancellationToken = default)
        {
            return new ValueTask<IAsyncEnumerable<int>>(Enumerable.Range(0, x).ToAsyncEnumerable());
        }

        var cts = new CancellationTokenSource();
        var result = await Enumerable.Range(0, 10)
            .ToAsyncEnumerable()
            //.Select(x => AsyncJob(x))
            .SelectAwaitWithCancellation(AsyncJob)
            //.SelectMany(x => Unfold(x))
            .SelectManyAwaitWithCancellation(Unfold)
            .ToArrayAsync(cts.Token);
    }

    [Test]
    public async Task AsyncEnumerableCreate()
    {
        var t = await Method1().ToArrayAsync();
        t = await AsyncEnumerable.Create(Method2).ToArrayAsync();
        //var source = AsyncEnumerable.Create(token => AsyncEnumerator.Create());
    }

    private async IAsyncEnumerable<int> Method1()
    {
        await Task.Delay(50);
        yield return 1;
    }

    private async IAsyncEnumerator<int> Method2(CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);
        yield return 1;
    }
}