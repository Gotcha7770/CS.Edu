using System;
using System.Collections.Generic;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class OnLoadedTests
    {
        private readonly SourceCache<Identity<Guid>, Guid> _cache = new SourceCache<Identity<Guid>, Guid>(x => x.Key);

        [TearDown]
        public void TearDown()
        {
            _cache.Clear();
        }

        [Test]
        public void OnLoadedEmptySource()
        {
            var list = new List<IChangeSet<Identity<Guid>, Guid>>();
            using (var aggregator = _cache.Connect()
                .OnLoaded(x => list.Add(x))
                .AsAggregator())
            {
                Assert.AreEqual(0, aggregator.Messages.Count);
                Assert.AreEqual(0, list.Count);
            }
        }

        [Test]
        public void OnLoadedWithInitialValues()
        {
            var items = new[]
            {
                new Identity<Guid>(Guid.NewGuid()),
                new Identity<Guid>(Guid.NewGuid())
            };
            _cache.AddOrUpdate(items);

            var list = new List<IChangeSet<Identity<Guid>, Guid>>();
            using (var aggregator = _cache.Connect()
                .OnLoaded(x => list.Add(x))
                .AsAggregator())
            {
                Assert.AreEqual(1, aggregator.Messages.Count);
                Assert.AreEqual(2, aggregator.Messages[0].Adds);
                Assert.AreEqual(1, list.Count);
                Assert.AreEqual(2, list[0].Adds);
            }
        }

        [Test]
        public void OnLoadedEmptySource_IgnoreUpdates()
        {
            var list = new List<IChangeSet<Identity<Guid>, Guid>>();
            using (var aggregator = _cache.Connect()
                .OnLoaded(x => list.Add(x))
                .AsAggregator())
            {
                _cache.AddOrUpdate(new Identity<Guid>(Guid.NewGuid()));

                Assert.AreEqual(1, aggregator.Messages.Count);
                Assert.AreEqual(1, aggregator.Messages[0].Adds);
                Assert.AreEqual(0, list.Count);
            }
        }

        [Test]
        public void OnLoadedWithInitialValues_IgnoreUpdates()
        {
            var items = new[]
            {
                new Identity<Guid>(Guid.NewGuid()),
                new Identity<Guid>(Guid.NewGuid())
            };
            _cache.AddOrUpdate(items);

            var list = new List<IChangeSet<Identity<Guid>, Guid>>();
            using (var aggregator = _cache.Connect()
                .OnLoaded(x => list.Add(x))
                .AsAggregator())
            {

                _cache.AddOrUpdate(new Identity<Guid>(Guid.NewGuid()));

                Assert.AreEqual(2, aggregator.Messages.Count);
                Assert.AreEqual(2, aggregator.Messages[0].Adds);
                Assert.AreEqual(1, aggregator.Messages[1].Adds);
                Assert.AreEqual(1, list.Count);
                Assert.AreEqual(2, list[0].Adds);
            }
        }
    }
}