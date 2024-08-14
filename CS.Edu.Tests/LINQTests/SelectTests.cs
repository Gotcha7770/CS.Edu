using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData.Kernel;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class SelectTests
{
    [Fact]
    public async Task AsyncSelector()
    {
        var syncQuery = from i in Enumerable.Range(0, 10)
            let r = 10
            select (i, r);

        // lack of await word in async LINQ query
        // var asyncQuery = from i in Enumerable.Range(0, 10).ToAsyncEnumerable()
        //                  let r = await ValueTask.FromResult(i)
        //                  select (r, i);
    }

    [Fact]
    public void ManyTypesOneInterface()
    {
        var source = Enumerable.Range(0, 10); // Sync items
        //var source = Enumerable.Range(0, 10).ToAsyncEnumerable(); // Async items, pull based
        //var source = Enumerable.Range(0, 10).ToObservable(); // Async items, push based
        //var source = Optional.Some(10);

        var result =
            from x in source
            from y in source
            select x * y;
    }
}