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