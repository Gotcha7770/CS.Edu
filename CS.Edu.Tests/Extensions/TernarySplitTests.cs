using NUnit.Framework;
using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class TernarySplitTests
    {
        Relation<int, int, int> isMonotone = (x, y, z) => x < y ? y < z : y > z;

        [Test]
        public void Split_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            var result = items.Split(isMonotone).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void SplitFirstDiminsion_OneElement_ReturnsThatElement()
        {
            var items = new int[] { 1 };
            var result = items.Split(isMonotone).ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void SplitSecondDimension_OneElement_ReturnsThatElement()
        {
            var items = new int[] { 1 };
            var result = items.Split(isMonotone)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void SplitFirstDiminsion_TwoElements_ReturnsTheseElements()
        {
            var items = new int[] { 1, 2 };
            var result = items.Split(isMonotone).ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2 }));
        }

        [Test]
        public void SplitSecondDimension_TwoElements_ReturnsTheseElements()
        {
            var items = new int[] { 1, 2 };
            var result = items.Split(isMonotone)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2 }));
        }

        [Test]
        public void SplitFirstDiminsion_WhilePrevLessThenNext_Returns3Groups()
        {
            var items = new int[] { 1, 3, 5, 4, 3, 4, 5};
            var result = items.Split(isMonotone).ToArray();

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 3, 5 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 4, 3 }));
            Assert.That(result[2], Is.EqualTo(new int[] { 4, 5 }));
        }

        [Test]
        public void SplitSecondDimension_WhilePrevLessThenNext_ReturnsTheseElements()
        {
            var items = new int[] { 1, 3, 5, 4, 3, 4, 5};
            var result = items.Split(isMonotone)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(3));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 3, 5 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 4, 3 }));
            Assert.That(result[2], Is.EqualTo(new int[] { 4, 5 }));
        }
    }
}
