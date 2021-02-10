using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Tests.Utils;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SchedulerTests
    {
        private readonly IObservable<int> _source = Observable.Range(0, 10);

        class State
        {
            public State(ManualResetEvent reset)
            {
                Reset = reset;
            }

            public ManualResetEvent Reset { get; }

            public int Id { get; set; } = -1;
        }

        [Test]
        public void NewThreadScheduler_Schedule()
        {
            var state1 = new State(new ManualResetEvent(false));
            var state2 = new State(new ManualResetEvent(false));

            NewThreadScheduler.Default.Schedule(state1, (_, st) =>
            {
                st.Id = Thread.CurrentThread.ManagedThreadId;
                st.Reset.Set();
                return Disposable.Empty;
            });

            NewThreadScheduler.Default.Schedule(state2, (_, st) =>
            {
                st.Id = Thread.CurrentThread.ManagedThreadId;
                st.Reset.Set();
                return Disposable.Empty;
            });

            WaitHandle.WaitAll(new WaitHandle[] {state1.Reset, state2.Reset});
            Assert.AreNotEqual(state1.Id, state2.Id);
        }

        [Test]
        public void NewThreadScheduler_ScheduleLongRunning()
        {
            var state1 = new State(new ManualResetEvent(false));
            var state2 = new State(new ManualResetEvent(false));

            NewThreadScheduler.Default.ScheduleLongRunning(state1, (st, _) =>
            {
                st.Id = Thread.CurrentThread.ManagedThreadId;
                st.Reset.Set();
            });

            NewThreadScheduler.Default.ScheduleLongRunning(state2, (st, _) =>
            {
                st.Id = Thread.CurrentThread.ManagedThreadId;
                st.Reset.Set();
            });

            WaitHandle.WaitAll(new WaitHandle[] {state1.Reset, state2.Reset});
            Assert.AreNotEqual(state1.Id, state2.Id);
        }

        [Test]
        public async Task ObserveOn_NewThreadScheduler_ShortTasksExecutesAsOne()
        {
            var res = await _source.ObserveOn(NewThreadScheduler.Default)
                .Select(x => (x, ThreadId: Thread.CurrentThread.ManagedThreadId))
                .ToArray();

            Assert.AreEqual(10, res.Length);
            EnumerableAssert.All(res, x => x.ThreadId == res[0].ThreadId);
        }

        [Test]
        public async Task ObserveOn_NewThreadScheduler_()
        {
            var res = await _source.ObserveOn(NewThreadScheduler.Default)
                .Do(_ => Thread.Sleep(TimeSpan.FromSeconds(2)))
                .Select(x => (x, ThreadId: Thread.CurrentThread.ManagedThreadId))
                .ToArray();

            Assert.AreEqual(10, res.Length);
            EnumerableAssert.All(res, x => x.ThreadId == res[0].ThreadId);
        }
    }
}