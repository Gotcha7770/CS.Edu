using System;
using System.Reactive.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class ComplexReactiveQueries
    {
        [Test]
        public void OneToOne()
        {
            string result = null;
            IObservable<int> source = Observable.Return(1);
            IObservable<string> selector = source.Select(x => x.ToString());

            using (_ = selector.Subscribe(x => result = x))
            {
                Assert.AreEqual("1", result);
            }
        }

        [Test]
        public void OneToMany()
        {
            IObservable<string> result = null;
            IObservable<int> source = Observable.Return(1);
            IObservable<IObservable<string>> selector = source.Select(x => Observable.Return(x.ToString()));

            using (_ = selector.Subscribe(x => result = x))
            {
                Assert.AreEqual("1", result.First());
            }
        }

        [Test]
        public void ManyToOne()
        {
            string result = null;
            IObservable<IObservable<string>> source = Observable.Return(1).Select(x => Observable.Return(x.ToString()));
            IObservable<string> selector = source.Merge(); //SelectMany, Switch

            using (_ = selector.Subscribe(x => result = x))
            {
                Assert.AreEqual("1", result);
            }
        }

        [Test]
        public void ManyToMany()
        {

        }
    }
}