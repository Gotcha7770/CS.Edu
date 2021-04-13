using System;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions.EnumerableExtensions;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions
{
    [TestFixture]
    public class TakeWhileTests
    {
        private readonly Relation<int> _lessThan = (x, y) => x < y;

        [Test]
        public void TakeWhile_PrevLessThenNext_ReturnsFirst3Elements()
        {
            var items = new[] {1, 2, 3, 2, 1};
            var result = items.TakeWhile(_lessThan);

            CollectionAssert.AreEqual(result, new[] {1, 2, 3});
        }

        [Test]
        public void TakeWhile_OneElement_ReturnsThatElement()
        {
            var result = EnumerableEx.Return(1).TakeWhile(_lessThan);

            CollectionAssert.AreEqual(result, new[] {1});
        }

        [Test]
        public void TakeWhile_Empty_ReturnsEmpty()
        {
            var result = Enumerable.Empty<int>().TakeWhile(_lessThan);

            CollectionAssert.AreEqual(result, Array.Empty<int>());
        }
    }
}