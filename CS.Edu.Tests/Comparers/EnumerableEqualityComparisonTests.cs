using System.Collections;
using System.Linq;
using CS.Edu.Core.Comparers;
using NUnit.Framework;

namespace CS.Edu.Tests.Comparers
{
    [TestFixture]
    public class EnumerableEqualityComparisonTests
    {
        [Test]
        public void BothEnumerableAreNull()
        {
            CollectionAssert.AreEqual(null, null);
            //Assert.IsTrue(one.SequenceEqual(other)); //throws exception if one is null
            Assert.IsTrue(EnumerableEqualityComparer<int>.Instance.Equals(null, null));
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(null, null));
        }

        [Test]
        public void OneOfEnumerableIsNull()
        {
            var one = Enumerable.Range(0, 3);

            CollectionAssert.AreNotEqual(one, null);
            //Assert.IsFalse(one.SequenceEqual(null)); //throws exception if other is null
            Assert.IsFalse(EnumerableEqualityComparer<int>.Instance.Equals(one, null));
            Assert.IsFalse(StructuralComparisons.StructuralEqualityComparer.Equals(one, null));
        }

        [Test]
        public void TwoEqualEnumerables_ReturnsTrue()
        {
            var one = Enumerable.Range(0, 3);
            var other = Enumerable.Range(0, 3);

            CollectionAssert.AreEqual(one, other);
            Assert.IsTrue(one.SequenceEqual(other));
            Assert.IsTrue(EnumerableEqualityComparer<int>.Instance.Equals(one, other));
            //Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(one, other)); //doesn't handle IEnumerable<T>
        }

        [Test]
        public void TwoNotEqualEnumerables_ReturnsFalse()
        {
            var one = Enumerable.Range(0, 3);
            var other = Enumerable.Range(0, 2);

            CollectionAssert.AreNotEqual(one, other);
            Assert.IsFalse(one.SequenceEqual(other));
            Assert.IsFalse(EnumerableEqualityComparer<int>.Instance.Equals(one, other));
            Assert.IsFalse(StructuralComparisons.StructuralEqualityComparer.Equals(one, other));
        }
    }
}