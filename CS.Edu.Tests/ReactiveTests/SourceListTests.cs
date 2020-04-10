using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using CS.Edu.Tests.Utils;
using DynamicData;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SourceListTests
    {
        [Test]
        public void SortTest()
        {
            var source = new SourceList<int>();

            var subscribtion = source
                .Connect()
                .Sort(Comparer<int>.Default)
                .Bind(out ReadOnlyObservableCollection<int> output)
                .Subscribe();

            source.Add(10);
            source.Add(2);
            source.Add(6);
            source.Add(4);
            source.Add(8);

            Assert.That(output, Is.EqualTo(new[] { 2, 4, 6, 8, 10 }));
        }

        [Test]
        public void ChangeTest()
        {
            var source = new SourceList<TestClass>();
            var results = new List<IChangeSet<TestClass>>();

            source.Connect().AutoRefresh(x => x.Value)
                .Subscribe(x => results.Add(x));

            var testObj = new TestClass();
            source.Add(testObj);

            testObj.Value = "newValue";

            Assert.IsTrue(true);
        }
    }
}