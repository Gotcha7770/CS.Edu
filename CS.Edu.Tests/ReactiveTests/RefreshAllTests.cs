using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class RefreshAllTests
    {
        private TestClass[] _items = Enumerable.Range(1, 100)
                .Select(i => new TestClass(i.ToString()))
                .ToArray();

        [Test]
        public void RefreshAllTest()
        {
            using(var refresher = new Subject<Unit>())
            using (var cache = new SourceCache<TestClass, string>(x => x.Value))
            using (var results = cache.Connect().AutoRefreshOnObservable(x => refresher).AsAggregator())
            {
                cache.AddOrUpdate(_items);
                refresher.OnNext(Unit.Default);

                Assert.AreEqual(101, results.Messages.Count);
                EnumerableAssert.All(results.Messages.SelectMany(x => x), x => x.Reason == ChangeReason.Refresh);
            }
        }
    }
}