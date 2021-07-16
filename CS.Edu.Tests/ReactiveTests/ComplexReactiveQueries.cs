using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Tests.Utils;
using DynamicData;
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
            ISubject<int> source = new BehaviorSubject<int>(0);

            // using (_ = source.Merge().Subscribe(x => result = x))
            // {
            //     Assert.AreEqual("1", result);
            // }
        }

        [Test]
        public void ManyToOne()
        {
            var result = new List<int>();
            ISubject<IObservable<int>> source = new Subject<IObservable<int>>();
            ISubject<int> even = new BehaviorSubject<int>(0);
            ISubject<int> odd = new BehaviorSubject<int>(1);

            using (_ = source.Merge().Subscribe(result.Add))
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