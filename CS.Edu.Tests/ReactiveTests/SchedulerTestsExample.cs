using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SchedulerTestsExample
    {
        [Test]
        public void TestingWithTestScheduler()
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

        [Test]
        public void RecordMessagesExample()
        {
            var scheduler = new TestScheduler();
            var source = Observable.Interval(TimeSpan.FromSeconds(1), scheduler)
                .Take(4);

            var testObserver = scheduler.Start(
                () => source,
                0,
                0,
                TimeSpan.FromSeconds(5).Ticks);

            Console.WriteLine("Time is {0} ticks", scheduler.Clock);
            Console.WriteLine("Received {0} notifications", testObserver.Messages.Count);
            
            foreach (Recorded<Notification<long>> message in testObserver.Messages)
            {
                Console.WriteLine("{0} @ {1}", message.Value, message.Time);
            }
        }
    }
}