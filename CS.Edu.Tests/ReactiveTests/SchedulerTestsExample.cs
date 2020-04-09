using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SchedulerTestsExample
    {
        [Test]
        public void Testing_with_test_scheduler()
        {
            var expectedValues = new long[] { 0, 1, 2, 3, 4 };
            var actualValues = new List<long>();
            var scheduler = new TestScheduler();
            var disposable = Observable
                .Interval(TimeSpan.FromSeconds(1), scheduler)
                .Take(5)
                .Subscribe(actualValues.Add);
                
            scheduler.Start();
            CollectionAssert.AreEqual(expectedValues, actualValues);
        }
    }
}