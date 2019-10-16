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
        private readonly IObservable<int> _observeable = Observable.Zip(
            Observable.Range(0, 100),
            Observable.Interval(TimeSpan.FromSeconds(0.5)),
            (item, timer) => item);

        [Test]
        public void ContinueWidthTest()
        {
            Task<(int, bool)> _current = Task.FromResult((0, false));
            ManualResetEvent mre = new ManualResetEvent(false);
            List<int> output = new List<int>();

            _observeable.Subscribe(
                x => _current = _current.ContinueWith(t => { output.Add(x); return (x, x.IsEven()); }),
                x => {},
                () => mre.Set());

            mre.WaitOne();

            Assert.That(output, Is.EqualTo(Enumerable.Range(0, 100)));
        }
    }
}