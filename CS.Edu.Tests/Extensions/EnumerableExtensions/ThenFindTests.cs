using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Extensions.EnumerableExtensions;
using DynamicData.Kernel;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions
{
    [TestFixture]
    public class ThenFindTests
    {
        private readonly IEnumerable<int> _source = Enumerable.Range(1, 99);

        [Test]
        public void FluentOneLayerFind()
        {
            Optional<int> result = _source.Find(x => x.IsEven()).Result();
            Assert.True(result.HasValue);
            Assert.AreEqual(2, result.Value);
        }

        [Test]
        public void FluentTwoLayersFind()
        {
            Optional<int> result = _source.Find(x => x == 101).ThenFind(x => x % 3 == 0).Result();
            Assert.True(result.HasValue);
            Assert.AreEqual(3, result.Value);
        }
    }
}