using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests;

[TestFixture]
public class TakeRangeTests
{
    [Test]
    public void TakeRange()
    {
        var items = Enumerable.Range(0, 20);
        var page1 = items.Skip(10).Take(20);
        var page2 = items.Take(10..30);
    }
}