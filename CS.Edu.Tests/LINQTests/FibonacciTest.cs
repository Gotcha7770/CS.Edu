using System;
using System.Collections.Generic;
using CS.Edu.Core.MathExt;
using NUnit.Framework;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class FibonacciTest
    {
        [Test]
        public void Test2()
        {
            int tenth1 = Fibonacci.Recursive(10);
            int tenth2 = Fibonacci.Iterator().ElementAt(9);
            int tenth3 = EnumerableExt.Generate((X: 0, Y: 1), t => (t.Y, t.X + t.Y)).ElementAt(9).X;
            int tenth4 = GetObservableFibonacci(10).Last();

            //Expand
            //Zip
            //Concat -> func(obj, IEnumerable<obj>) =>
            //Publish???
            //Create
            //Defer
            //Observable

            Assert.That(tenth1, Is.EqualTo(34));
            Assert.That(tenth2, Is.EqualTo(34));
            Assert.That(tenth3, Is.EqualTo(34));
            Assert.That(tenth3, Is.EqualTo(34));
        }

        [Test]
        public void Method1()
        {
            var res = GetObservableFibonacci(10).ToEnumerable();

            CollectionAssert.AreEqual(new [] {0, 1, 1, 2, 3, 5, 8, 13, 21, 34}, res);
        }

        private IObservable<int> GetObservableFibonacci(int count)
        {
            return Observable.Create<int>(observer =>
            {
                var storage = new BehaviorSubject<int>(1);
                var output = Observable.Return(0).Concat(storage).Take(count);
                var transfer = output.Zip(storage, (x, y) => x + y);

                return new CompositeDisposable
                {
                    output.Subscribe(observer),
                    transfer.Subscribe(storage),
                    storage
                };
            });
        }
    }
}