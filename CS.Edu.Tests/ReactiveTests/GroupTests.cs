﻿using System;
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
    public class GroupTests
    {
        private Groupable<int, string>[] _items = Enumerable.Range(1, 10)
            .Select(x => new Groupable<int, string>(x, "Group1"))
            .Concat(Enumerable.Range(1, 10).Select(x => new Groupable<int, string>(x, "Group2")))
            .Concat(Enumerable.Range(1, 10).Select(x => new Groupable<int, string>(x, "Group3")))
            .Concat(Enumerable.Range(1, 10).Select(x => new Groupable<int, string>(x, "Group4")))
            .ToArray();

        [Test]
        public void GroupTest_RefreshItemWithoutGroupKeyChanged()
        {
            using (var cache = new SourceCache<Groupable<int, string>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.GroupKey)
                .AsAggregator())
            {
                cache.AddOrUpdate(_items);
                cache.Refresh(_items[0]);

                Assert.AreEqual(1, aggregator.Messages.Count);
                EnumerableAssert.None(aggregator.Messages.SelectMany(x => x), x => x.Reason == ChangeReason.Refresh);
            }
        }

        [Test]
        public void GroupTest_RefreshGroupsWithoutGroupKeyChanged()
        {
            using (var refresher = new Subject<Unit>())
            using (var cache = new SourceCache<Groupable<int, string>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.GroupKey, refresher)
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
            using (var cache = new SourceCache<Groupable<int, string>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.GroupKey)
                .AsAggregator())
            {
                cache.AddOrUpdate(_items);
                cache.AddOrUpdate(new Groupable<int, string>(1, "Group 2"));

                Assert.AreEqual(2, aggregator.Messages.Count);
            }
        }

        [Test]
        public void GroupTest_Update()
        {
            using (var cache = new SourceCache<Groupable<int, string>, Guid>(x => x.Key))
            using (var aggregator = cache.Connect()
                .Group(x => x.GroupKey)
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