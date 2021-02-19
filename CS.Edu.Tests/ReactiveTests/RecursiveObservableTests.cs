using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class RecursiveObservableTests
    {
        [Test]
        public void Test1()
        {
            IObservable<int> CreateRecursive()
            {
                var one = new BehaviorSubject<int>(0);
                var other = new Subject<int>();
                return Observable.Create<int>(observer => new CompositeDisposable
                    {
                        one.Subscribe(observer),
                        other.Subscribe(one),
                        one.Select(x => ++x).Subscribe(other),
                        other,
                        one
                    }
                );
            };

            var res = CreateRecursive().Take(5).ToEnumerable();

            CollectionAssert.AreEqual(new [] {0, 1, 2, 3, 4}, res); //Stack overflow
        }

        [Test]
        public void Test2()
        {
            IObservable<int> CreateRecursive()
            {
                var source = new BehaviorSubject<int>(0);
                var transfer = source.Select(x => ++x);

                return Observable.Create<int>(observer => new CompositeDisposable
                {
                    source.Subscribe(observer),
                    transfer.Subscribe(source)
                });
            }

            var res = CreateRecursive().Take(5).ToEnumerable();

            CollectionAssert.AreEqual(new [] {0, 1, 2, 3, 4}, res); //Stack overflow
        }

        [Test]
        public void Test3()
        {
            IObservable<int> CreateRecursive(int count)
            {
                var source = new BehaviorSubject<int>(0);
                var transfer = source.Select(x => ++x);
                var guard = source.Skip(count).Subscribe(_ => source.OnCompleted());

                return Observable.Create<int>(observer => new CompositeDisposable
                {
                    source.Subscribe(observer),
                    transfer.Subscribe(source),
                    guard
                });
            }

            var res = CreateRecursive(5).ToEnumerable();

            CollectionAssert.AreEqual(new [] {0, 1, 2, 3, 4}, res);
        }

        [Test]
        public void Test4()
        {
            IObservable<int> CreateRecursive(int count)
            {
                var source = new BehaviorSubject<int>(0);
                var transfer = source.Take(count - 1).Select(x => ++x);

                return Observable.Create<int>(observer => new CompositeDisposable
                {
                    source.Subscribe(observer),
                    transfer.Subscribe(source)
                });
            }

            var res = CreateRecursive(5).ToEnumerable();

            CollectionAssert.AreEqual(new [] {0, 1, 2, 3, 4}, res);
        }
    }
}