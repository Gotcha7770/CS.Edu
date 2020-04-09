using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData;
using NUnit.Framework;

namespace CS.Edu.Tests.ReactiveTests
{
    [TestFixture]
    public class SortChangeSetTests
    {
        [Test]
        public void SortTest()
        {
            ReadOnlyObservableCollection<int> output;
            var changeSet = new SourceList<int>();
            var subscribtion = changeSet
                .Connect()
                .Sort(Comparer<int>.Default)
                .Bind(out output)
                .Subscribe();

            changeSet.Add(10);
            changeSet.Add(2);
            changeSet.Add(6);
            changeSet.Add(4);
            changeSet.Add(8);

            Assert.That(output, Is.EqualTo(new[] { 2, 4, 6, 8, 10 }));
        }
    }
}