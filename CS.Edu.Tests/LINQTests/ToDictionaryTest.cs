using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class ToDictionaryTest
    {
        private readonly (int Key, string Value)[] _items =
        {
            (0, "first"),
            (1, "first"),
            (0, "second"),
            (2, "first"),
            (0, "third"),
            (1, "second")
        };

        [Test]
        public void ToDictionaryWithGroupBy()
        {
            var dic = _items.GroupBy(x => x.Key)
                .Select(x => x.Last())
                .ToDictionary(x => x.Key, x => x.Value);

            Assert.AreEqual(3, dic.Keys.Count);
            Assert.AreEqual("third", dic[0]);
            Assert.AreEqual("second", dic[1]);
            Assert.AreEqual("first", dic[2]);
        }

        [Test]
        public void ToDictionaryWithoutGroupBy()
        {
            var dic = _items.ToDictionary(x => x.Key, x => x.Value);

            Assert.AreEqual(3, dic.Keys.Count);
            Assert.AreEqual("third", dic[0]);
            Assert.AreEqual("second", dic[1]);
            Assert.AreEqual("first", dic[2]);
        }
    }
}