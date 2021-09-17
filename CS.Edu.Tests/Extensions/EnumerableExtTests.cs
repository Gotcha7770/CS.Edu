using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using EnumerableEx = System.Linq.EnumerableEx;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class EnumerableExtTests
    {
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
                (X: 0, Y: -9999),
                (X: 5, Y: -9999),
                (X: 10, Y: 0),
                (X: 20, Y: 1),
                (X: 30, Y: -9999),
                (X: 40, Y: 4),
                (X: 50, Y: -9999),
                (X: 60, Y: -9999),
                (X: 70, Y: -9999),
                (X: 80, Y: -9999),
                (X: 90, Y: 0),
                (X: 100, Y: 0)
            };

            var standard = new[]
            {
                (X: 0, Y: -9999),
                (X: 10, Y: 0),
                (X: 20, Y: 1),
                (X: 30, Y: -9999),
                (X: 40, Y: 4),
                (X: 50, Y: -9999),
                (X: 90, Y: 0),
                (X: 100, Y: 0)
            };

            var result = points.ShrinkDuplicates(x => x.Y, -9999);

            CollectionAssert.AreEqual(result, standard);
        }

        [Test]
        public void ToTreeTest()
        {
            // IEnumerable<Parameter> source = new[]
            // {
            //     new Parameter("Param1", new[] {"Cat1", "Cat4"}),
            //     new Parameter("Param2", new[] {"Cat1", "Cat5"}),
            //     new Parameter("Param3", new[] {"Cat2", "Cat4"}),
            //     new Parameter("Param4", new[] {"Cat2"}),
            //     new Parameter("Param5", new[] {"Cat3"}),
            //     new Parameter("Param6", Array.Empty<string>())
            // };
            //
            // string KeySelector(Parameter p, int i) => p.Categories.Skip(i).DefaultIfEmpty(string.Empty).First();
            //
            // Group<string, Parameter> ResultSelector(IGrouping<string, Parameter> grouping, int depth)
            // {
            //     return grouping.Skip(1).IsEmpty()
            //         ? new Group<string, Parameter>(grouping.Key, grouping.Take(1))
            //         : new Group<string, Parameter>(grouping.Key, grouping.GroupBy(x => KeySelector(x, depth + 1)).Select(x => ResultSelector(x, depth + 1)));
            // }
            //
            // var result = source.GroupBy(x => KeySelector(x, 0)).Select(x => ResultSelector(x, 0)).ToArray();

            //IEnumerable<Group<TKey, T>> ToTree<TKey, T>(IEnumerable<T> source, Func<T, TKey> keySelector)
            // IEnumerable<Group<TKey, T>> ToTree<TKey, T>(IEnumerable<T> source)
            // {
            //     //var tmp = source.GroupBy(keySelector, (k, e) => ToTree(e, keySelector, keyModifier));
            //     source.GroupBy(x => x).ForEach(x => x.);
            //
            //     return Enumerable.Empty<Group<TKey, T>>();
            // }
            //
            // IEnumerable<Group<string, Parameter>> ToTree(IEnumerable<Parameter> source)
            // {
            //     //var tmp = source.GroupBy(keySelector, (k, e) => ToTree(e, keySelector, keyModifier));
            //     int index = 0;
            //     string KeySelector(Parameter p, int i) => p.Categories.Skip(i).DefaultIfEmpty(string.Empty).First();
            //
            //     Group<string, Parameter> resultSelector(IGrouping<string, Parameter> grouping) => new Group<string, Parameter>(grouping.Key, grouping);
            //
            //     return source.GroupBy(x => KeySelector(x, index)).Select(x => x.ToGroup())
            // }

            //var result1 = source.GroupBy(x => x.Categories.IsEmpty() ? string.Empty : x.Categories.First(), x => x.Categories.Skip(1));
            //var result2 = ToTree(source, x => x.Categories.IsEmpty() ? string.Empty : x.Categories.First(), x => x.Categories.Skip(1));
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