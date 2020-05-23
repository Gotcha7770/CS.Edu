using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        static readonly (int, int)[] Standard =
        {
            (0, 0),
            (0, 1),
            (1, 0),
            (1, 1),
        };

        static readonly int[] Left = {0, 1};
        static readonly int[] Right = {0, 1};

        [Test]
        public void EnumerableCartesian()
        {
            var cartesian = from x in Left
                            from y in Right
                            select (x, y);

            // foreach(var x in Left)
            // {
            //     foreach (var y in Right)
            //     {
            //         result.Add((x, y));
            //     }
            // }

            //cartesian = Left.SelectMany(x => Right.Select(y => (x, y)))

            CollectionAssert.AreEqual(cartesian, Standard);
        }

        [Test]
        public void ObservableCartesianCold()
        {
            var product = from x in Left.ToObservable()
                          from y in Right.ToObservable()
                          select (x, y);

            var cartesian = product.ToEnumerable();

            CollectionAssert.AreEqual(cartesian, Standard);
        }

        [Test]
        public void ObservableCartesianHot()
        {
            Subject<int> left = new Subject<int>(); //IObservable<int>
            Subject<int> right = new Subject<int>();

            var cartesian = new List<(int, int)>();

            var product = from x in left
                          from y in right
                          select (x, y);

            var subscription = product
                .Subscribe(x => cartesian.Add(x));

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
            IObservable<IChangeSet<int>> one = Left.AsObservableChangeSet();
            IObservable<IChangeSet<int>> other = Right.AsObservableChangeSet();

            var subscription = one.Product(other, (x, y) => (x, y))
                .Bind(out ReadOnlyObservableCollection<(int, int)> cartesian)
                .Subscribe();

            var product = from x in one
                          from y in other
                          select (x, y);

            //IObservable<(IChangeset<T1>, IChangeset<T2>) -> IObservable<IChangeset<(T1, T2)>>

            // var subscription = one.TransformMany<(int, int), int>(x => Transformation(x, other))
            //     .Bind(out ReadOnlyObservableCollection<(int, int)> cartesian)
            //     .Subscribe();

            CollectionAssert.AreEqual(cartesian, Standard);
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

            left.AddRange(new [] {0, 1});

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