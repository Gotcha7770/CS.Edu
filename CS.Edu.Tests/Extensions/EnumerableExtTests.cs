using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtTests
    {
        [Test]
        public void NullOrEmpty_Null_ReturnsTrue()
        {
            IEnumerable<int> empty = null;
            Assert.That(empty.IsNullOrEmpty(), Is.True);
        }

        [Test]
        public void NullOrEmpty_Empty_ReturnsTrue()
        {
            var empty = Array.Empty<int>();
            Assert.That(empty.IsNullOrEmpty(), Is.True);
        }

        [Test]
        public void NullOrEmpty_NotEmpty_ReturnsFalse()
        {
            var items = Enumerable.Range(0, 85);
            Assert.That(items.IsNullOrEmpty(), Is.False);
        }

        [Test]
        public void Paginate_Enumerable_ReturnsEnumerableOfEnumerable()
        {
            int pageSize = 20;
            var items = Enumerable.Range(0, 85);
            var paginated = items.Paginate(pageSize).ToArray();
            Assert.That(paginated, Is.Not.Empty);
            Assert.That(paginated.Length, Is.EqualTo(((items.Count() - 1) / pageSize) + 1));
            Assert.That(paginated.First(), Is.InstanceOf<IEnumerable>());
            Assert.That(paginated.First().Count(), Is.EqualTo(pageSize));
        }

        [Test]
        public void Paginate_EmptyEnumerable_ReturnsEmptyEnumerable()
        {
            var items = Array.Empty<int>();
            var paginated = items.Paginate(5);
            Assert.That(paginated, Is.Not.Null);
            Assert.That(paginated, Is.Empty);
        }
        [Test]
        public void Paginate_Null_ThrowsArgumentNullException()
        {
            IEnumerable<int> empty = null;
            Assert.Throws<ArgumentNullException>(() => empty.Paginate(2));
        }
        [Test]
        public void Paginate_PageSizeIs0_ThrowsArgumentOutOfRangeException()
        {
            var items = Enumerable.Range(0, 10);
            Assert.Throws<ArgumentOutOfRangeException>(() => items.Paginate(0));
        }

        [Test]
        public void TakeLastLinkedListTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastLinkedList(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new [] {990, 991, 992, 993, 994, 995, 996, 997, 998, 999}));
        }

        [Test]
        public void TakeLastReverseTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastReverse(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new[] { 990, 991, 992, 993, 994, 995, 996, 997, 998, 999 }));
        }
    }
}
