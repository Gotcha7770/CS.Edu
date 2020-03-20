using NUnit.Framework;
using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class BinarySplitTests
    {
        public Relation<int> lessThan = (x, y) => x < y;

        [Test]
        public void Split_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            var result = items.Split(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void SplitFirstDimension_OneElement_ReturnsThatElement()
        {
            var items = new int[] { 1 };
            var result = items.Split(lessThan).ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void SplitSecondDimension_OneElement_ReturnsThatElement()
        {
            var items = new int[] { 1 };
            var result = items.Split(lessThan)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void SplitFirstDimension_WhilePrevLessThenNext_Returns2Groups()
        {
            var items = new int[] { 1, 2, 3, 2, 3 };
            var result = items.Split(lessThan)
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2, 3 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 2, 3 }));
        }

        [Test]
        public void SplitSecondDimension_WhilePrevLessThenNext_Returns2Groups()
        {
            var items = new int[] { 1, 2, 3, 2, 3 };
            var result = items.Split(lessThan)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2, 3 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 2, 3 }));
        }
    }
}