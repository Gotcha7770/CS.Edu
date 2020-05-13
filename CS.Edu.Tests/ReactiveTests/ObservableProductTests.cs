using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using CS.Edu.Core.Extensions;
using DynamicData;
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
    public class ObservableProductTests
    {
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

            var subscription = one.Product(other, (x, y) => (x, y))
                .Bind(out ReadOnlyObservableCollection<(int, int)> cartesian)
                .Subscribe();

            CollectionAssert.AreEqual(cartesian, standard);
        }

        [Test]
        public void ObservableListCartesian()
        {
            SourceList<int> left = new SourceList<int>();
            SourceList<int> right = new SourceList<int>();

            var subscription = left.Connect()
                .Product(right.Connect(), (l, r) => (l, r))
                .Bind(out ReadOnlyObservableCollection<(int, int)> cartesian)
                .Subscribe();

            CollectionAssert.IsEmpty(cartesian);

            left.AddRange(Enumerable.Range(0, 2));

            CollectionAssert.IsEmpty(cartesian);

            right.Add(0);

            CollectionAssert.AreEqual(cartesian, new[] {(0, 0), (1, 0)});

            right.Add(1);

            CollectionAssert.AreEqual(cartesian, new[] {(0, 0), (1, 0), (0, 1), (1, 1)});

            left.Remove(0);

            CollectionAssert.AreEqual(cartesian, new[] {(1, 0), (1, 1)});

            right.Clear();

            CollectionAssert.IsEmpty(cartesian);
        }

        [Test]
        public void DynamicDataJoinTest()
        {
            SourceCache<Curve, Guid> left = new SourceCache<Curve, Guid>(x => x.Id);
            SourceCache<Curve, Guid> right = new SourceCache<Curve, Guid>(x => x.Id);

            var result = left.Connect()
                .Group(x => x.WellId)
                .LeftJoinMany<IGroup<Curve, Guid, int>, int, Curve, Guid, IEnumerable<CurveRelation>>(right.Connect(), x => x.WellId, (l, r) => Product1(l.Cache.Items, r.Items))
                .Bind(out ReadOnlyObservableCollection<IEnumerable<CurveRelation>> relations)
                //.Bind(out ReadOnlyObservableCollection<CurveRelation> relations)
                .Subscribe();

            var subscription = right.Connect()
                .Bind(out ReadOnlyObservableCollection<Curve> curves)
                .Subscribe();

            left.Edit(innerCache =>
            {
                innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
                innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
            });

            // right.Edit(innerCache =>
            // {
            //     innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
            //     innerCache.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
            // });

            right.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });
            right.AddOrUpdate(new Curve { Id = Guid.NewGuid(), WellId = 1 });


            Assert.IsTrue(true);
            // [x1, y1], [x2, y1]
        }

        private IEnumerable<CurveRelation> Product1(IEnumerable<Curve> one, IEnumerable<Curve> other)
        {
            return from x in one
                from y in other
                select new CurveRelation(x, y);
        }

        private IObservable<IChangeSet<CurveRelation>> Product(IEnumerable<Curve> one, IEnumerable<Curve> other)
        {
            return ObservableChangeSet.Create<CurveRelation>(list =>
            {
                list.AddRange(Product1(one, other));
                return new CompositeDisposable();
            });
        }
    }
}