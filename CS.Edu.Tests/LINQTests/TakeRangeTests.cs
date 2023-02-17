using System.Linq;
using Xunit;

namespace CS.Edu.Tests.LINQTests;

public class TakeRangeTests
{
    [Fact]
    public void TakeRange()
    {
        var items = Enumerable.Range(0, 20);
        var page1 = items.Skip(10).Take(20);
        var page2 = items.Take(10..30);
    }
}