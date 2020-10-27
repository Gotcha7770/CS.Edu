﻿using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Iterators;
using CS.Edu.Tests.Utils;
using NUnit.Framework;

namespace CS.Edu.Tests.IteratorsTests
{
    [TestFixture]
    public class PrevNextIteratorTests
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
            var first = EnumerableEx.Return(0).ToPrevNextIterator().First();
            var standard = new PrevNextValue<int>(Option.None, 0, Option.None);

            Assert.AreEqual(standard, first);
        }

        [Test]
        public void SourceWithManyValues_ReturnsFirstItemWithNextValue()
        {
            var first = _items.ToPrevNextIterator().First();
            var standard = new PrevNextValue<int>(Option.None, 0, 1);

            Assert.AreEqual(standard, first);
        }

        [Test]
        public void Test()
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
        public void LastTest()
        {
            var first = _items.ToPrevNextIterator().Last();
            var standard = new PrevNextValue<int>(3, 4, Option.None);

            Assert.AreEqual(standard, first);
        }
    }
}