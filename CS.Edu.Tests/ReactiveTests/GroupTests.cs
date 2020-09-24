using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class GroupTests
    {
        private Valuable<bool>[] _items = Enumerable.Range(1, 10)
            .Select(i => new Valuable<bool>(i.IsEven()))
            .ToArray();

        [Test]
        public void GroupTest_RefreshItem()
        {
            using (var cache = new SourceCache<Valuable<bool>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.Value)
                .AsAggregator())
            {
                cache.AddOrUpdate(_items);
                cache.Refresh(_items[0]);

                Assert.AreEqual(1, aggregator.Messages.Count);
                EnumerableAssert.None(aggregator.Messages.SelectMany(x => x), x => x.Reason == ChangeReason.Refresh);
            }
        }

        [Test]
        public void GroupTest_RefreshGroups()
        {
            using (var refresher = new Subject<Unit>())
            using (var cache = new SourceCache<Valuable<bool>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.Value, refresher)
                .AsAggregator())
            {
                cache.AddOrUpdate(_items);
                refresher.OnNext(Unit.Default);

                Assert.AreEqual(1, aggregator.Messages.Count);
                EnumerableAssert.None(aggregator.Messages.SelectMany(x => x), x => x.Reason == ChangeReason.Refresh);
            }
        }

        [Test]
        public void GroupTest_Add()
        {
            using (var cache = new SourceCache<Valuable<bool>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.Value)
                .AsAggregator())
            {
                cache.AddOrUpdate(_items);
                cache.AddOrUpdate(new Valuable<bool>(false));

                Assert.AreEqual(1, aggregator.Messages.Count);
            }
        }

        [Test]
        public void GroupTest_Update()
        {
            using (var cache = new SourceCache<Valuable<bool>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.Value)
                .AsAggregator())
            {
                cache.AddOrUpdate(_items);
                cache.AddOrUpdate(_items[0]);

                Assert.AreEqual(2, aggregator.Messages.Count);
                EnumerableAssert.All(aggregator.Messages.SelectMany(x => x), x => x.Reason == ChangeReason.Refresh);
            }
        }
    }
}