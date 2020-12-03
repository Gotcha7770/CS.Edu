using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.TaskTests
{
    public class ContinueWidth
    {
        private readonly IObservable<long> _observable = Observable.Interval(TimeSpan.FromSeconds(0.5))
            .Take(100);

        [Test]
        public void ContinueWidthTest()
        {
            Task<(long, bool)> current = Task.FromResult((0L, false));
            ManualResetEvent mre = new ManualResetEvent(false);
            List<long> output = new List<long>();

            _observable.Subscribe(
                x => current = current.ContinueWith(t => { output.Add(x); return (x, x.IsEven()); }),
                x => {},
                () => mre.Set());

            mre.WaitOne();

            Assert.That(output, Is.EqualTo(Enumerable.Range(0, 100)));
        }

        [Test]
        public void ContinueWidthTest_CanceledTask()
        {
            int result = 0;
            Task<int> current = Task.FromCanceled<int>(new CancellationToken(true));

            Assert.DoesNotThrowAsync(async () =>
            {
                var continuation = current.ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                        result = 42;
                });

                await continuation;
            });

            Assert.AreEqual(0, result);
        }
    }
}