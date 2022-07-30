using System;
using System.Collections.Generic;
using System.Linq;
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
        public void SelectManyTest2()
        {
            var result1 = from x in Enumerable.Range(0, 2)
                          from y in Enumerable.Range(0, 2)
                          from z in Enumerable.Range(0, 2)
                          select x + y + z;

            var result2 = Enumerable.Range(0, 2)
                .SelectMany(x => Enumerable.Range(0, 2), (x, y) => new { x, y })
                .SelectMany(t => Enumerable.Range(0, 2), (t, z) => t.x + t.y + z);
        }
    }
}