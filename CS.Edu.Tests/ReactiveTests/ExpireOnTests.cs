using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Extensions.ObservableExtensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class ExpireOnTests
    {
        private readonly Subject<Unit> _subject = new Subject<Unit>();
        private readonly SourceCache<Valuable<int>, Guid> _source = new SourceCache<Valuable<int>, Guid>(x => x.Key);

        [TearDown]
        public void TearDown()
        {
            _source.Clear();
        }

        [Test]
        public void RemoveAllWithFilter()
        {
            _source.AddOrUpdate(new Valuable<int>(0));
            _source.AddOrUpdate(new Valuable<int>(1));

            using (var aggregate = _source.Connect()
                .Filter(_subject.Select(_ => Predicates.False<Valuable<int>>()).StartWith(Predicates.True<Valuable<int>>()))
                .SkipInitial()
                .AsAggregator())
            {
                _subject.OnNext(Unit.Default);
                Assert.AreEqual(1, aggregate.Messages.Count);
                Assert.AreEqual(2, aggregate.Messages[0].Removes);
            }
        }

        [Test]
        public void ExpireOn_Initial()
        {
            using (var aggregate = _source.Connect()
                .ExpireOn(_subject)
                .AsAggregator())
            {
                _source.AddOrUpdate(new Valuable<int>(0));
                _source.AddOrUpdate(new Valuable<int>(1));

                Assert.AreEqual(2, aggregate.Messages.Count);
                Assert.AreEqual(1, aggregate.Messages[0].Adds);
                Assert.AreEqual(1, aggregate.Messages[1].Adds);
            }
        }

        [Test]
        public void ExpireOn_ClearsOnEvaluation()
        {
            _source.AddOrUpdate(new Valuable<int>(0));
            _source.AddOrUpdate(new Valuable<int>(1));

            using (var aggregate = _source.Connect()
                .ExpireOn(_subject)
                .SkipInitial()
                .AsAggregator())
            {
                _subject.OnNext(Unit.Default);
                Assert.AreEqual(1, aggregate.Messages.Count);
                Assert.AreEqual(2, aggregate.Messages[0].Removes);
            }
        }

        [Test]
        public void ExpireOn_EmptySource_NothingToClear()
        {
            using (var aggregate = _source.Connect()
                .ExpireOn(_subject)
                .AsAggregator())
            {
                _subject.OnNext(Unit.Default);
                Assert.AreEqual(0, aggregate.Messages.Count);
            }
        }
    }
}