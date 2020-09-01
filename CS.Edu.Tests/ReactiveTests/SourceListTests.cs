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
        public void WatchPropertyTest()
        {
            var source = new SourceList<Valuable<string>>();
            var testObj = new Valuable<string>("value");
            source.Add(testObj);

            ChangeSetAggregator<Valuable<string>> results;

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
            var source = new SourceList<Valuable<string>>();
            var testObj = new Valuable<string>("value");
            source.Add(testObj);

            var changeHistory = new List<string>();
            ChangeSetAggregator<Valuable<string>> results;

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