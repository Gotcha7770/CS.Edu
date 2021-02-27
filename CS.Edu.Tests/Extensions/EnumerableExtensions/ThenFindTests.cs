using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Extensions.EnumerableExtensions;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions
{
    [TestFixture]
    public class ThenFindTests
    {
        private readonly IEnumerable<int> _source = Enumerable.Range(1, 99);

        [Test]
        public void FluentFindTest()
        {
            int result = _source.Find(x => x.IsEven()).Match(l => -1, Function.Identity<int>());
            Assert.AreEqual(2, result);

            result = _source.Find(x => x == 101).ThenFind(x => x % 3 == 0).Match(l => -1, Function.Identity<int>());
            Assert.AreEqual(3, result);
        }
    }
}