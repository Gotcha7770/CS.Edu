using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class ListExtTests
    {
        [Test]
        public void Paginate_Enumerable_ReturnsEnumerableOfEnumerable()
        {
            int pageSize = 20;
            var items = Enumerable.Range(0, 85).ToList();
            var paginated = items.Split(pageSize).ToArray();
            Assert.That(paginated, Is.Not.Empty);
            Assert.That(paginated.Length, Is.EqualTo(((items.Count() - 1) / pageSize) + 1));
            Assert.That(paginated.First(), Is.InstanceOf<IEnumerable>());
            Assert.That(paginated.First().Count(), Is.EqualTo(pageSize));
        }

        [Test]
        public void Paginate_EmptyEnumerable_ReturnsEmptyEnumerable()
        {
            var items = Array.Empty<int>().ToList();
            var paginated = items.Split(5);
            Assert.That(paginated, Is.Not.Null);
            Assert.That(paginated, Is.Empty);
        }
        [Test]
        public void Paginate_Null_ThrowsArgumentNullException()
        {
            List<int> empty = null;
            Assert.Throws<ArgumentNullException>(() => empty.Split(2));
        }
        [Test]
        public void Paginate_PageSizeIs0_ThrowsArgumentOutOfRangeException()
        {
            var items = Enumerable.Range(0, 10).ToList();
            Assert.Throws<ArgumentOutOfRangeException>(() => items.Split(0));
        }
    }
}
