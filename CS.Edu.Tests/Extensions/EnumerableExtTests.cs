using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;

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
        public void Except_Null_ThrowsArgumentNullException()
        {
            IEnumerable<int> empty = null;
            Assert.Throws<ArgumentNullException>(() => empty.Except(0).ToArray()); //???
        }

        [Test]
        public void Except_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            Assert.That(items.Except(0), Is.Empty);
        }

        [Test]
        public void Except_OnlyTargetItem_ReturnsEmpty()
        {
            var items = Enumerable.Range(0, 1);
            Assert.That(items.Except(0), Is.Empty);
        }

        [Test]
        public void Except_EnumerableWithTargetItem_ReturnsWithoutTargetItem()
        {
            var items = Enumerable.Range(0, 3);
            var result = items.Except(1).ToArray();
            Assert.That(result, Is.EqualTo(new[] { 0, 2 }));
        }

        [Test]
        public void Except_EnumerableWithoutTargetItem_ReturnsSource()
        {
            var items = Enumerable.Range(0, 5);
            var result = items.Except(10).ToArray();
            Assert.That(result, Is.EqualTo(new[] { 0, 1, 2, 3, 4 }));
        }

        [Test]
        public void TakeWhile_PrevLessThenNext_ReturnsFirst3Elements()
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
        public void SkipWhile_PrevLessThenNext_SkipFirst2Elements()
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

        [Test]
        public void Paginate_MaterializePartsOfEnumeration_ReturnsCorrectValues()
        {
            var items = Enumerable.Range(0, 1000);

            var paginated = items.Paginate(25);
            var first = paginated.Skip(5).First().ToArray();
            var second = paginated.Skip(10).First().ToArray();
            var third = paginated.Skip(1).First().First();

            Assert.That(first, Is.EqualTo(Enumerable.Range(125, 25)));
            Assert.That(second, Is.EqualTo(Enumerable.Range(250, 25)));
            Assert.That(third, Is.EqualTo(25));
        }
    }
}
