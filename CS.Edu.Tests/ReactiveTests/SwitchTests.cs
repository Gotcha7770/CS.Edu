using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using DynamicData.Tests;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SwitchTests
    {
        [Test]
        public void SelectManyTest()
        {
            //SelectMany is just: source.Select(selector).Merge();

            var scheduler = new TestScheduler();
            var one = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 0),
                ReactiveTest.OnNext(20, 0),
                ReactiveTest.OnNext(30, 0),
                ReactiveTest.OnNext(40, 0),
                ReactiveTest.OnCompleted<int>(50));

            var other = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 1),
                ReactiveTest.OnNext(20, 1),
                ReactiveTest.OnNext(30, 1),
                ReactiveTest.OnNext(40, 1),
                ReactiveTest.OnCompleted<int>(50));

            var outer = scheduler.CreateHotObservable(ReactiveTest.OnNext<IObservable<int>>(5, one),
                ReactiveTest.OnNext<IObservable<int>>(15, other),
                ReactiveTest.OnNext<IObservable<int>>(25, one),
                ReactiveTest.OnNext<IObservable<int>>(35, other),
                ReactiveTest.OnCompleted<IObservable<int>>(60));

            var res = scheduler.Start(() => outer.SelectMany(x => x), 0, 0, ReactiveTest.Disposed);

            res.Messages.AssertEqual(
                ReactiveTest.OnNext(10, 0),
                ReactiveTest.OnNext(20, 0),
                ReactiveTest.OnNext(20, 1),
                ReactiveTest.OnNext(30, 0),
                ReactiveTest.OnNext(30, 0),
                ReactiveTest.OnNext(30, 1),
                ReactiveTest.OnNext(40, 0),
                ReactiveTest.OnNext(40, 0),
                ReactiveTest.OnNext(40, 1),
                ReactiveTest.OnNext(40, 1),
                ReactiveTest.OnCompleted<int>(60));
        }

        [Test]
        public void SwitchTest()
        {
            var scheduler = new TestScheduler();
            var one = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 0),
                ReactiveTest.OnNext(20, 0),
                ReactiveTest.OnNext(30, 0),
                ReactiveTest.OnNext(40, 0),
                ReactiveTest.OnCompleted<int>(50));

            var other = scheduler.CreateHotObservable(ReactiveTest.OnNext(10, 1),
                ReactiveTest.OnNext(20, 1),
                ReactiveTest.OnNext(30, 1),
                ReactiveTest.OnNext(40, 1),
                ReactiveTest.OnCompleted<int>(50));

            var outer = scheduler.CreateHotObservable(ReactiveTest.OnNext<IObservable<int>>(5, one),
                ReactiveTest.OnNext<IObservable<int>>(15, other),
                ReactiveTest.OnNext<IObservable<int>>(25, one),
                ReactiveTest.OnNext<IObservable<int>>(35, other),
                ReactiveTest.OnCompleted<IObservable<int>>(60));

            var res = scheduler.Start(() => outer.Switch(), 0, 0, ReactiveTest.Disposed);

            res.Messages.AssertEqual(
                ReactiveTest.OnNext(10, 0),
                ReactiveTest.OnNext(20, 1),
                ReactiveTest.OnNext(30, 0),
                ReactiveTest.OnNext(40, 1),
                ReactiveTest.OnCompleted<int>(60));
        }

        [Test]
        public void SwitchOnTest()
        {
            SourceList<int> one = new SourceList<int>();
            SourceList<int> other = new SourceList<int>();

            one.AddRange(new[] {1, 3});
            other.AddRange(new[] {2, 4});

            using (var switcher = new BehaviorSubject<Unit>(Unit.Default))
            using(var aggregate = switcher.Scan(other, (agg, _) => agg == one ? other : one).Switch().AsAggregator())
            {
                CollectionAssert.AreEqual(new [] {1, 3}, aggregate.Data.Items);

                switcher.OnNext(Unit.Default);

                CollectionAssert.AreEqual(new [] {2, 4}, aggregate.Data.Items);

                one.Add(5);

                CollectionAssert.AreEqual(new [] {2, 4}, aggregate.Data.Items);

                switcher.OnNext(Unit.Default);

                CollectionAssert.AreEqual(new [] {1, 3, 5}, aggregate.Data.Items);

                other.Remove(4);

                CollectionAssert.AreEqual(new [] {1, 3, 5}, aggregate.Data.Items);

                one.Remove(3);

                CollectionAssert.AreEqual(new [] {1, 5}, aggregate.Data.Items);
            }
        }
    }
}