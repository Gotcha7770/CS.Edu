using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using CS.Edu.Tests.Utils;
using CS.Edu.Core.Extensions;
using DynamicData;
using DynamicData.Binding;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class AggregateTests
    {
        [Test]
        public void AnyTest()
        {
            bool aggregate = false;
            using(var subject = new Subject<bool>())
            using (var subscription = subject
                .Any(x => x)
                .Subscribe(x => aggregate = x))
            {
                subject.OnNext(false);

                Assert.IsFalse(aggregate);

                subject.OnNext(true);

                Assert.IsTrue(aggregate);
            }
        }

        [Test]
        public void TrueForAnyTest()
        {
            bool aggregate = false;

            using(var cache = new SourceCache<Valuable<bool>, Guid>(x => x.Key))
            using (var subscription = cache.Connect()
                .TrueForAny(x => x.WhenValueChanged(p => p.Value), Function.Identity<bool>())
                .Subscribe(x => aggregate = x))
            {
                cache.AddOrUpdate(new Valuable<bool>(false));

                Assert.IsFalse(aggregate);

                cache.AddOrUpdate(new Valuable<bool>(true));

                Assert.IsTrue(aggregate);
            }
        }
    }
}