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
            var empty = Enumerable.Empty<int>();
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
            var items = new int[] { 1, 2, 3, 2, 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.TakeWhile(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(new int[] { 1, 2, 3 }));
        }

        [Test]
        public void TakeWhile_OneElement_ReturnsThatElement()
        {
            var items = new int[] { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.TakeWhile(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void TakeWhile_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.TakeWhile(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void SkipWhile_WhilePrevLessThenNext_SkipFirst2Elements()
        {
            var items = new int[] { 1, 2, 3, 2, 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.SkipWhile(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(new int[] { 2, 1 }));
        }

        [Test]
        public void SkipWhile_OneElement_ReturnsEmpty()
        {
            var items = new List<int> { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.SkipWhile(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void SkipWhile_Empty_ReturnsEmpty()
        {
            var items = Array.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.SkipWhile(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void Split_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.Split(lessThan).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void Split_OneElement_ReturnsOneArrayWithThatElement()
        {
            var items = new int[] { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.Split(lessThan).ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void Split_WhilePrevLessThenNext_Returns2Groups()
        {
            var items = new int[] { 1, 2, 3, 2, 3 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.Split(lessThan).ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2, 3 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 2, 3 }));
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
            var items = Enumerable.Empty<int>();

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
    }
}
