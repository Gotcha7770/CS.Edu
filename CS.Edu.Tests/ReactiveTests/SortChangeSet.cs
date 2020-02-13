using System;
using System.Collections.Generic;
using DynamicData;
using DynamicData.Binding;
using NUnit.Framework;

namespace CS.Edu.Tests.ExpandTests
{
    [TestFixture]
    public class SortChangeSetTests
    {
        [Test]
        public void SortTest()
        {
            var output = new ObservableCollectionExtended<int>();
            var changeSet = new SourceList<int>();
            var subscribtion = changeSet
                .Connect()
                .Sort(Comparer<int>.Default)
                .Bind(output)
                .Subscribe();

            changeSet.Add(10);
            changeSet.Add(2);
            changeSet.Add(6);
            changeSet.Add(4);
            changeSet.Add(8);

            Assert.That(output, Is.EqualTo(new [] {2, 4, 6, 8, 10}));
        }
    }
}