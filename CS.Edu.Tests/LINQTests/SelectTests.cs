using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class SelectTests
    {
        [Test]
        public void SelectManyTest()
        {
            //SelectMany is just: source.Select(selector).Concat()

            IEnumerable<IEnumerable<int>> source = new []
            {
                new []{0, 1, 2},
                new []{3, 4, 5}
            };

            Func<IEnumerable<int>, IEnumerable<string>> selector = x => x.Select(i => i.ToString());

            CollectionAssert.AreEqual(source.SelectMany(selector), source.Select(selector).Concat());
        }

        [Test]
        public void SelectVsSelectMany()
        {
            IEnumerable<int> items = Enumerable.Range(0, 100)
                .Buffer(25)
                .Select((x, i) => i.IsEven() ? x : x.Reverse())
                .SelectMany(x => x)
                .ToArray();

            IEnumerable<int> items2 = Enumerable.Range(0, 100)
                .Buffer(25)
                .SelectMany((x, i) => i.IsEven() ? x : x.Reverse())
                .ToArray();

            Assert.That(items, Is.EqualTo(items2));
        }
    }
}