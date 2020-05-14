using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Core.Extensions;
using DynamicData;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
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
        public void ObservableCartesianCold()
        {
            var standard = new[]
            {
                (0, 0),
                (1, 0),
                (0, 1),                
                (1, 1),
            };

            var cartesian = new List<(int, int)>();

            IObservable<int> one = Enumerable.Range(0, 2).ToObservable();
            IObservable<int> other = Enumerable.Range(0, 2).ToObservable();

            var subscription = one.GroupJoin(other,
                          _ => Observable.Never<Unit>(),
                          _ => Observable.Never<Unit>(),
                          (l, r) => (Left: l, Right: r))
            .Subscribe(x =>
            {
                x.Right.Subscribe(r =>
                {
                    cartesian.Add((x.Left, r));
                });
            });

            CollectionAssert.AreEqual(cartesian, standard);
        }

        [Test]
        public void ObservableCartesianHot()
        {
            Subject<int> left = new Subject<int>();
            Subject<int> right = new Subject<int>();

            var cartesian = new List<(int, int)>();

            var subscription = left.GroupJoin(right,
                          _ => Observable.Never<Unit>(),
                          _ => Observable.Never<Unit>(),
                          (l, r) => (Left: l, Right: r))
            .Subscribe(x =>
            {
                x.Right.Subscribe(r =>
                {
                    cartesian.Add((x.Left, r));
                });
            });

            CollectionAssert.IsEmpty(cartesian);

            left.OnNext(0);
            left.OnNext(1);

            CollectionAssert.IsEmpty(cartesian);

            right.OnNext(0);

            CollectionAssert.AreEqual(cartesian, new[] { (0, 0), (1, 0) });

            right.OnNext(1);

            CollectionAssert.AreEqual(cartesian, new[] { (0, 0), (1, 0), (0, 1), (1, 1) });
        }

        [Test]
        public void ObservableChangeSetCartesianCold()
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
        public void ObservableChangeSetCartesianHot()
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

            CollectionAssert.AreEqual(cartesian, new[] { (0, 0), (1, 0) });

            right.Add(1);

            CollectionAssert.AreEqual(cartesian, new[] { (0, 0), (1, 0), (0, 1), (1, 1) });

            left.Remove(0);

            CollectionAssert.AreEqual(cartesian, new[] { (1, 0), (1, 1) });

            right.Clear();

            CollectionAssert.IsEmpty(cartesian);
        }
    }
}