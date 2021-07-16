using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DynamicData;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SharedSourceListTest
    {
        [Test]
        public void Aggregate_ReferenceTypeAccumulatorProblem()
        {
            var source = new Subject<IChangeSet<int>>();
            var target1 = new BehaviorSubject<IList<int>>(null);
            var target2 = new BehaviorSubject<IList<int>>(null);

            var observable = source.Aggregate(new List<int>(),
                (acc, changes) =>
                {
                    acc.Clone(changes);
                    return acc;
                });

            using var s1 = observable.Subscribe(target1);
            using var s2 = observable.Subscribe(target2);

            source.OnNext(new ChangeSet<int> {new Change<int>(ListChangeReason.Add, 1, 0)});
            source.OnCompleted();

            Assert.AreEqual(new[] {1}, target1.Value);
        }

        [Test]
        public void Aggregate_Fix1()
        {
            var source = new Subject<IChangeSet<int>>();
            var target1 = new BehaviorSubject<IList<int>>(null);
            var target2 = new BehaviorSubject<IList<int>>(null);

            var observable = Observable.Create<IList<int>>(observer =>
            {
                return source.Aggregate(new List<int>(),
                        (acc, changes) =>
                        {
                            acc.Clone(changes);
                            return acc;
                        })
                    .Subscribe(observer);
            });

            using var s1 = observable.Subscribe(target1);
            using var s2 = observable.Subscribe(target2);

            source.OnNext(new ChangeSet<int> {new Change<int>(ListChangeReason.Add, 1, 0)});
            source.OnCompleted();

            Assert.AreEqual(new[] {1}, target1.Value);
        }

        [Test]
        public void Aggregate_Fix2()
        {
            var source = new Subject<IChangeSet<int>>();
            var target1 = new BehaviorSubject<IList<int>>(null);
            var target2 = new BehaviorSubject<IList<int>>(null);

            var observable = source.Aggregate(new List<int>(),
                (acc, changes) =>
                {
                    var copy = acc.ToList();
                    copy.Clone(changes);
                    return copy;
                });

            using var s1 = observable.Subscribe(target1);
            using var s2 = observable.Subscribe(target2);

            source.OnNext(new ChangeSet<int> {new Change<int>(ListChangeReason.Add, 1, 0)});
            source.OnCompleted();

            Assert.AreEqual(new[] {1}, target1.Value);
        }

        [Test]
        public void Scan()
        {
            var source = new Subject<IChangeSet<int>>();
            var target1 = new BehaviorSubject<IList<int>>(null);
            var target2 = new BehaviorSubject<IList<int>>(null);

            var observable = source.Scan(new List<int>(),
                (list, changes) =>
                {
                    list.Clone(changes);
                    return list;
                });

            using var s1 = observable.Subscribe(target1);
            using var s2 = observable.Subscribe(target2);

            source.OnNext(new ChangeSet<int> {new Change<int>(ListChangeReason.Add, 1, 0)});
        }

        [Test]
        public void QueryWhenChanged()
        {
            var source = new SourceList<int>();
            var observable = source.Connect()
                .QueryWhenChanged();

            var first = new BehaviorSubject<IReadOnlyCollection<int>>(null);
            using var s1 = observable.Subscribe(first);

            var second = new BehaviorSubject<IReadOnlyCollection<int>>(null);
            using var s2 = observable.Subscribe(second);
        }

        // [Test]
        // public void ObservableQueryAreIndependentAndInvokeConnectManyTimes()
        // {
        //     string result1 = null;
        //     string result2 = null;
        //
        //     var observable = _source.Connect()
        //         .ToCollection()
        //         .Select(x => string.Join('+', x));
        //
        //     using var s1 = observable.Subscribe(x => result1 = x);
        //     using var s2 = observable.Subscribe(x => result2 = x);
        //
        //     _source.Add(4);
        //     _source.Add(5);
        // }
    }
}