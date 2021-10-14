using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class DelayEachItemTests
    {
        [Test]
        public void DelayEachItem()
        {
            var scheduler = new TestScheduler();
            var source = scheduler.CreateHotObservable(ReactiveTest.OnNext(20, 1),
                ReactiveTest.OnNext(30, 2),
                ReactiveTest.OnNext(40, 3),
                ReactiveTest.OnNext(50, 4),
                ReactiveTest.OnCompleted<int>(60));

            //should subscribe in 10 ticks to avoid shift
            var result = scheduler.Start(() => Observable.Interval(TimeSpan.FromTicks(20), scheduler).Zip(source, (_, x) => x), 0, 10, ReactiveTest.Disposed);

            result.Messages.AssertEqual(
                ReactiveTest.OnNext(30, 1),
                ReactiveTest.OnNext(50, 2),
                ReactiveTest.OnNext(70, 3),
                ReactiveTest.OnNext(90, 4),
                ReactiveTest.OnCompleted<int>(110));
        }
    }
}