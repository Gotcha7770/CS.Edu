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
        private readonly IObservable<long> _observeable = Observable.Interval(TimeSpan.FromSeconds(0.5))
            .Take(100);

        [Test]
        public void ContinueWidthTest()
        {
            Task<(long, bool)> _current = Task.FromResult((0L, false));
            ManualResetEvent mre = new ManualResetEvent(false);
            List<long> output = new List<long>();

            _observeable.Subscribe(
                x => _current = _current.ContinueWith(t => { output.Add(x); return (x, x.IsEven()); }),
                x => {},
                () => mre.Set());

            mre.WaitOne();

            Assert.That(output, Is.EqualTo(Enumerable.Range(0, 100)));
        }
    }
}