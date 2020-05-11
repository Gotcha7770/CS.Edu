using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Kernel;
using DynamicData.Tests;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    class Curve
    {
        public Guid Id { get; set; }

        public int WellId { get; set; }
    }

    class CurveRelation
    {
        public CurveRelation(Curve x, Curve y)
        {
            CurveX = x.Id;
            CurveY = y.Id;
        }

        public Guid CurveX {get; set;}
        public Guid CurveY {get; set;}
    }

    [TestFixture]
    public class ObservableJoinTests
    {
        private readonly SourceCache<Curve, Guid> _left = new SourceCache<Curve, Guid>(x => x.Id);
        private readonly SourceCache<Curve, Guid> _right = new SourceCache<Curve, Guid>(x => x.Id);
        private ChangeSetAggregator<CurveRelation, int> _result;

        [Test]
        public void EnumerableCartesian()
        {
            var standard = new[]
            {
                (0, 0),
                (0, 1),
                (1, 0),
                (1, 1),
            };

            IEnumerable<int> one = Enumerable.Range(0, 2);
            IEnumerable<int> other = Enumerable.Range(0, 2);

            var cartesian = from x in one
                from y in other
                select (x, y);

            CollectionAssert.AreEqual(cartesian, standard);
        }

        [Test]
        public void ChangeSetsCartesian()
        {
            var standard = new[]
            {
                (0, 0),
                (0, 1),
                (1, 0),
                (1, 1),
            };

            IObservable<IChangeSet<int>> one = Enumerable.Range(0, 2).AsObservableChangeSet();
            IObservable<IChangeSet<int>> other = Enumerable.Range(0, 2).AsObservableChangeSet();

            var subscription = one.Merge(other)
                .Bind(out ReadOnlyObservableCollection<int> cartesian)
                .Subscribe();

            CollectionAssert.AreEqual(cartesian, standard);
        }

        [Test]
        public void DynamicDataJoinTest()
        {
            // _result = _left.Connect()
            //     .FullJoinMany(_right.Connect(), x => x.WellId, (l, g) => g.Items.Select(x => l.HasValue ? Optional.Some(new CurveRelation(x, l.Value)) : Optional<CurveRelation>.None))
            //     .Filter(x => x.HasValue)
            //     .AsAggregator();

            var lGrouping = _left.Connect()
                .Group(x => x.WellId)
                .AsObservableCache();

            var rGrouping = _right.Connect()
                .Group(x => x.WellId)
                .AsObservableCache();;

            // var result = _left.Connect()
            //     .Group(x => x.WellId)
            //     .LeftJoinMany<IGroup<Curve, Guid, int>, int, Curve, Guid, IObservable<IChangeSet<CurveRelation, Guid>>>(_right.Connect(), x => x.WellId, (l, r) => Product(l, r))
            //     .TransformMany(x => x.C, x => x.Changes)
            //     .AsAggregator();

            _left.Edit(innerCache =>
            {
                innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
                innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
            });

            _right.Edit(innerCache =>
            {
                innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
                innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 2 });
            });


            Assert.IsTrue(true);
            // [x1, y1], [x2, y1]
        }

        // private IObservable<IChangeSet<CurveRelation, Guid>> Product(IGroup<Curve,Guid,int> @group, IGrouping<Curve,Guid,int> grouping)
        // {
        // }
    }
}