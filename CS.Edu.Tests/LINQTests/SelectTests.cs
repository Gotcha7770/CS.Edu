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
    }
}