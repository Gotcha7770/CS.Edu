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
        public void TakeWhile_WhilePrevLessThenNext_ReturnsFirst3Elements()
        {
            var items = new List<int> { 1, 2, 3, 2, 1 };
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.TakeWhile(lessThan).ToArray(), Is.EqualTo(new int[] { 1, 2, 3 }));
        }

        [Test]
        public void TakeWhile_OneElement_ReturnsThatElement()
        {
            var items = new List<int> { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.TakeWhile(lessThan).ToArray(), Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void TakeWhile_Empty_ReturnsEmpty()
        {
            var items = Array.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.TakeWhile(lessThan).ToArray(), Is.EqualTo(new int[0]));
        }

        [Test]
        public void SkipWhile_WhilePrevLessThenNext_SkipFirst2Elements()
        {
            var items = new List<int> { 1, 2, 3, 2, 1 };
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.SkipWhile(lessThan).ToArray(), Is.EqualTo(new int[] { 2, 1 }));
        }

        [Test]
        public void SkipWhile_OneElement_ReturnsEmpty()
        {
            var items = new List<int> { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.SkipWhile(lessThan).ToArray(), Is.EqualTo(new int[0]));
        }

        [Test]
        public void SkipWhile_Empty_ReturnsEmpty()
        {
            var items = Array.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.SkipWhile(lessThan).ToArray(), Is.EqualTo(new int[0]));
        }

        [Test]
        public void Split_Empty_ReturnsEmpty()
        {
            var items = Array.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            Assert.That(items.Split(lessThan).ToArray(), Is.EqualTo(new int[0]));
        }

        [Test]
        public void Split_OneElement_ReturnsEmpty() //???
        {
            var items = new List<int> { 1 };
            Relation<int> lessThan = (x, y) => x < y;
        }

        [Test]
        public void Split_WhilePrevLessThenNext_Returns2Groups()
        {
            var items = new List<int> { 1, 2, 3, 2, 3 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.Split(lessThan).ToArray();
            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2, 3 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 2, 3 }));
        }

        [Test]
        public void SplitTest()
        {
            var items = new List<int> { 88, 89, 90, 90, 90, 91, 92, 90, 90, 90, 89, 88, 90};

            Relation<int> isDirectionNotChanged = (x, y) =>
            {
                return x > 90 && y > 90 || x < 90 && y < 90;
            };

            var result = items.Split(isDirectionNotChanged).Where(x => x.All(y => y != 90)).ToArray();
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
        public void TakeLastArrayTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastArray(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new[] { 990, 991, 992, 993, 994, 995, 996, 997, 998, 999 }));
        }

        [Test]
        public void TakeLastListTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastList(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new[] { 990, 991, 992, 993, 994, 995, 996, 997, 998, 999 }));
        }

        [Test]
        public void TakeLastLinkedListTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastLinkedList(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new[] { 990, 991, 992, 993, 994, 995, 996, 997, 998, 999 }));
        }

        [Test]
        public void TakeLastReverseTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastReverse(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new[] { 990, 991, 992, 993, 994, 995, 996, 997, 998, 999 }));
        }

        [Test]
        public void TakeLastSpanTest()
        {
            var items = Enumerable.Range(0, 1000);
            var lastTen = items.TakeLastSpan(10).ToArray();

            Assert.That(lastTen.Length, Is.EqualTo(10));
            Assert.That(lastTen, Is.EquivalentTo(new[] { 990, 991, 992, 993, 994, 995, 996, 997, 998, 999 }));
        }
    }
}
