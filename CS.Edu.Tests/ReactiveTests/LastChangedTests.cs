using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using CS.Edu.Tests.Utils;
using DynamicData;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class LastChangedTests
    {
        [Test]
        public void LastOrDefaultTest()
        {
            int lastOrDefault = 0;
            var source = new SourceList<int>();

            var autoSelector = source.Connect()
                .ToCollection()
                .Subscribe(items => lastOrDefault = items.LastOrDefault());

            Assert.AreEqual(lastOrDefault, 0);

            source.Add(42);
            Assert.AreEqual(lastOrDefault, 42);

            source.Remove(42);
            Assert.AreEqual(lastOrDefault, 0);
        }

        [Test]
        public void LastChangedTest()
        {
            Selectable<object> selected = null;

            var first = new Selectable<object>();
            var second = new Selectable<object>();
            var third = new Selectable<object>();

            var source = new SourceCache<Selectable<object>, Guid>(x => x.Key);
            source.AddOrUpdate(new[] { first, third });

            var autoSelector = source.Connect()
                .AutoRefresh(x => x.IsSelected)
                .Flatten()
                .Select(change => change.Current)
                .Where(x => x.IsSelected)
                .Subscribe(latest => selected = latest);

            Assert.IsNull(selected);

            first.IsSelected = true;
            Assert.AreSame(selected, first);

            third.IsSelected = true;
            Assert.AreSame(selected, third);

            source.AddOrUpdate(second);
            Assert.AreSame(selected, third);

            second.IsSelected = true;
            Assert.AreSame(selected, second);
        }

        [Test]
        public void LastChangedOrDefaultTest()
        {
            Selectable<object> selected = null;

            var first = new Selectable<object>();
            var second = new Selectable<object> { IsSelected = true };
            var source = new SourceCache<Selectable<object>, Guid>(x => x.Key);

            var autoSelector = source.Connect()
                .AutoRefresh(x => x.IsSelected)
                .ToCollection()
                .Subscribe(items => selected = items.LastOrDefault(x => x.IsSelected));

            source.AddOrUpdate(first);
            Assert.IsNull(selected);

            first.IsSelected = true;
            Assert.AreSame(selected, first);

            source.AddOrUpdate(second);
            Assert.AreSame(selected, second);

            second.IsSelected = false;
            Assert.AreSame(selected, first);

            first.IsSelected = false;
            Assert.IsNull(selected);
        }
    }
}