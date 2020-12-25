using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using CS.Edu.Core.Helpers;
using CS.Edu.Tests.Utils;
using DynamicData.Kernel;
using EnumerableEx = System.Linq.EnumerableEx;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtTests
    {
        [Test]
        public void NullOrEmpty_Null_ReturnsTrue()
        {
            IEnumerable<int> empty = null;
            Assert.IsTrue(empty.IsNullOrEmpty());
        }

        [Test]
        public void NullOrEmpty_Empty_ReturnsTrue()
        {
            var empty = Enumerable.Empty<int>();
            Assert.IsTrue(empty.IsNullOrEmpty());
        }

        [Test]
        public void NullOrEmpty_NotEmpty_ReturnsFalse()
        {
            var items = Enumerable.Range(0, 85);
            Assert.False(items.IsNullOrEmpty());
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
            CollectionAssert.IsEmpty(items.Except(0));
        }

        [Test]
        public void Except_OnlyTargetItem_ReturnsEmpty()
        {
            var items = EnumerableEx.Return(0);
            CollectionAssert.IsEmpty(items.Except(0));
        }

        [Test]
        public void Except_EnumerableWithTargetItem_ReturnsWithoutTargetItem()
        {
            var items = Enumerable.Range(0, 3);
            var result = items.Except(1);

            CollectionAssert.AreEqual(result, new[] {0, 2});
        }

        [Test]
        public void Except_EnumerableWithoutTargetItem_ReturnsSource()
        {
            var items = Enumerable.Range(0, 5);
            var result = items.Except(10);

            CollectionAssert.AreEqual(result, new[] {0, 1, 2, 3, 4});
        }

        [Test]
        public void TakeWhile_PrevLessThenNext_ReturnsFirst3Elements()
        {
            var items = new[] {1, 2, 3, 2, 1};
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.TakeWhile(lessThan);

            CollectionAssert.AreEqual(result, new[] {1, 2, 3});
        }

        [Test]
        public void TakeWhile_OneElement_ReturnsThatElement()
        {
            var items = EnumerableEx.Return(1);
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.TakeWhile(lessThan);

            CollectionAssert.AreEqual(result, new[] {1});
        }

        [Test]
        public void TakeWhile_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.TakeWhile(lessThan);

            CollectionAssert.AreEqual(result, Array.Empty<int>());
        }

        [Test]
        public void SkipWhile_PrevLessThenNext_SkipFirst2Elements()
        {
            var items = new int[] {1, 2, 3, 2, 1};
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.SkipWhile(lessThan);

            CollectionAssert.AreEqual(result, new[] {2, 1});
        }

        [Test]
        public void SkipWhile_OneElement_ReturnsEmpty()
        {
            var items = new List<int> {1};
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.SkipWhile(lessThan);

            CollectionAssert.AreEqual(result, (Array.Empty<int>()));
        }

        [Test]
        public void SkipWhile_Empty_ReturnsEmpty()
        {
            var items = Array.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            var result = items.SkipWhile(lessThan);

            CollectionAssert.AreEqual(result, Array.Empty<int>());
        }

        [Test]
        public void Paginate_Enumerable_ReturnsEnumerableOfEnumerable()
        {
            int pageSize = 20;
            var items = Enumerable.Range(0, 85);

            var paginated = items.Paginate(pageSize).ToArray();

            CollectionAssert.IsNotEmpty(paginated);
            Assert.AreEqual(paginated.Length, 5);
            Assert.AreEqual(paginated.First().Count(), pageSize);
        }

        [Test]
        public void Paginate_EmptyEnumerable_ReturnsEmptyEnumerable()
        {
            var items = Enumerable.Empty<int>();

            var paginated = items.Paginate(5);

            Assert.IsNotNull(paginated);
            CollectionAssert.IsEmpty(paginated);
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

            CollectionAssert.AreEqual(first, Enumerable.Range(125, 25));
            CollectionAssert.AreEqual(second, Enumerable.Range(250, 25));
            Assert.AreEqual(third, 25);
        }

        [Test]
        public void ShrinkDuplicatesTest()
        {
            var points = new[] {0, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0};

            var result = points.ShrinkDuplicates(1);

            CollectionAssert.AreEqual(result, new[] {0, 1, 0, 0, 1, 0, 1, 0});
        }

        [Test]
        public void ShrinkDuplicatesWithKeySelectorTest()
        {
            var points = new[]
            {
                new Point {X = 0, Y = -9999}, new Point {X = 5, Y = -9999}, new Point {X = 10, Y = 0},
                new Point {X = 20, Y = 1}, new Point {X = 30, Y = -9999}, new Point {X = 40, Y = 4},
                new Point {X = 60, Y = -9999}, new Point {X = 70, Y = -9999}, new Point {X = 80, Y = -9999},
                new Point {X = 90, Y = 0}, new Point {X = 100, Y = 0},
            };

            var standard = new[]
            {
                new Point {X = 0, Y = -9999}, new Point {X = 10, Y = 0}, new Point {X = 20, Y = 1},
                new Point {X = 30, Y = -9999}, new Point {X = 40, Y = 4}, new Point {X = 60, Y = -9999},
                new Point {X = 90, Y = 0}, new Point {X = 100, Y = 0},
            };

            var result = points.ShrinkDuplicates(x => x.Y, -9999);

            CollectionAssert.AreEqual(result, standard);
        }

        [Test]
        public void Enumerator_CreateTests()
        {
            IEnumerable<int> _0 = new [] {1, 2};

            IEnumerable<int> _1 = EnumerableEx.Concat(
                EnumerableEx.Return(1),
                EnumerableEx.Return(2));

            IEnumerable<int> _2 = EnumerableEx.Create<int>(async yield =>
            {
                await yield.Return(1);
                await yield.Return(2);
            });

            IEnumerable<int> _3 = EnumerableEx.Create(() => Enumerator.Create(0, 1));

            IEnumerable<int> _4 = _(); IEnumerable<int> _()
            {
                yield return 1;
                yield return 2;
            }

            Assert.True(true);
        }

        [Test]
        public void FluentFindTest()
        {
            IEnumerable<int> source = Enumerable.Range(1, 99);

            int result = source.Find(x => x.IsEven(), true).Result.Value;
            Assert.AreEqual(2, result);

            result = source.Find(x => x == 101, true).ThenFind(x => x % 3 == 0).Result.Value;
            Assert.AreEqual(3, result);
        }

        [Test]
        public void ParallelFindTest()
        {
            IEnumerable<int> source = Enumerable.Range(1, 99);

            int result = FindMethod(source, x => x.IsEven()).Value;
            Assert.AreEqual(2, result);

            result = FindMethod(source, x => x == 101, x => x % 3 == 0).Value;
            Assert.AreEqual(3, result);
        }

        private Optional<T> FindMethod<T>(IEnumerable<T> items, params Predicate<T>[] predicates)
        {
            var results = new Optional<T>[predicates.Length];
            Parallel.ForEach(predicates, (cur, state, index) =>
            {
                using (var enumerator = items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (cur(enumerator.Current))
                        {
                            results[index] = enumerator.Current;

                            if (index == 0)
                                state.Stop();

                            break;
                        }

                        if (state.IsStopped)
                            break;
                    }
                }
            });

            return results.FirstOrDefault(x => x.HasValue);
        }

        [TestCaseSource(typeof(ExceptIfLastTestsDataSource), nameof(ExceptIfLastTestsDataSource.TestCases))]
        public IEnumerable<int> Test(IEnumerable<int> input)
        {
            return input.ExceptIfLast(1);
        }

        internal class ExceptIfLastTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(Enumerable.Empty<int>()).Returns(Enumerable.Empty<int>());
                    yield return new TestCaseData(EnumerableEx.Return(0)).Returns(EnumerableEx.Return(0));
                    yield return new TestCaseData(EnumerableEx.Return(1)).Returns(Enumerable.Empty<int>());
                    yield return new TestCaseData(new[] {0, 1}).Returns(EnumerableEx.Return(0));
                    yield return new TestCaseData(new[] {1, 1}).Returns(EnumerableEx.Return(1));
                    yield return new TestCaseData(new[] {1, 0}).Returns(new[] {1, 0});
                    yield return new TestCaseData(new[] {1, 0, 1}).Returns(new[] {1, 0});
                    yield return new TestCaseData(new[] {1, 0, 1, 0}).Returns(new[] {1, 0, 1, 0});
                }
            }
        }
    }
}