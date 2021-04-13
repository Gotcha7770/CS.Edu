using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions.EnumerableExtensions;
using CS.Edu.Core.Iterators;
using DynamicData.Kernel;
using NUnit.Framework;
using EnumerableEx = System.Linq.EnumerableEx;

namespace CS.Edu.Tests.Extensions.EnumerableExtensions
{
    [TestFixture]
    public class PrevNextTests
    {
        private readonly IEnumerable<int> _items = Enumerable.Range(0, 5);

        [Test]
        public void EmptySource_ReturnsEmptyIterator()
        {
            var iterator = Enumerable.Empty<int>().ToPrevNextIterator();

            CollectionAssert.IsEmpty(iterator);
        }

        [Test]
        public void SourceWithSingleValue_ReturnsJustCurrent()
        {
            var iterator = EnumerableEx.Return(0).ToPrevNextIterator();
            var standard = new[] {new PrevNextValue<int>(Optional<int>.None, 0, Optional<int>.None)};

            CollectionAssert.AreEqual(iterator, standard);
        }

        [Test]
        public void SourceWithManyValues_ReturnsFirstItemWithNextValue()
        {
            var first = _items.ToPrevNextIterator().First();
            var standard = new PrevNextValue<int>(Optional<int>.None, 0, 1);

            Assert.AreEqual(standard, first);
        }

        [Test]
        public void MiddleItems_AllItemsHasPreviousAndNextValues()
        {
            var iterator = _items.ToPrevNextIterator().Skip(1).Take(3);
            var standard = new[]
            {
                new PrevNextValue<int>(0, 1, 2),
                new PrevNextValue<int>(1, 2, 3),
                new PrevNextValue<int>(2, 3, 4)
            };

            CollectionAssert.AreEqual(iterator, standard);
        }

        [Test]
        public void LastItemWithNoneNextValue()
        {
            var first = _items.ToPrevNextIterator().Last();
            var standard = new PrevNextValue<int>(3, 4, Optional<int>.None);

            Assert.AreEqual(standard, first);
        }
    }
}