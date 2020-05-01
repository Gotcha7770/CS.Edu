using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Tests.Utils;
using DynamicData;
using DynamicData.Tests;
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
        public void WatchPropertyTest()
        {
            var source = new SourceList<TestClass>();
            var testObj = new TestClass();
            source.Add(testObj);

            ChangeSetAggregator<TestClass> results;

            results = source.Connect()
                .AutoRefresh(x => x.Value)
                .AsAggregator();

            testObj.Value = "newValue";

            var refreshes = results.Messages
                .SelectMany(x => x)
                .Where(x => x.Reason == ListChangeReason.Refresh);

            Assert.That(refreshes, Has.Exactly(1).Items);
        } 

        [Test]   
        public void SubscribeManyTest()
        {
            var source = new SourceList<TestClass>();
            var testObj = new TestClass();
            source.Add(testObj);

            var changeHistory = new List<string>();
            ChangeSetAggregator<TestClass> results;

            results = source.Connect()
                .SubscribeMany(data => ObservableExt.CreateFromProperty(data, x => x.Value)
                    .Subscribe(x => changeHistory.Add(x)))
                .AsAggregator();

            testObj.Value = "newValue";
            testObj.Value = "anotherNewValue";

            source.Remove(testObj);

            testObj.Value = "lastValue";

            Assert.IsTrue(true);
        }    
    }
}