using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Iterators;
using NUnit.Framework;

namespace CS.Edu.Tests.IteratorsTests
{
    [TestFixture]
    public class TrimIteratorTests
    {
        [TestCaseSource(typeof(TrimIteratorTestCaseSource), nameof(TrimIteratorTestCaseSource.TestCases))]
        public IEnumerable<int> TrimStartIteratorTests(IEnumerable<int> source)
        {
            return TrimStartIterator(source, 0);
        }

        // [Test]
        // public void Test()
        // {
        //     IEnumerable<int> source = Enumerable.Repeat(0, 5)
        //         .Concat(Enumerable.Range(0, 10))
        //         .Concat(Enumerable.Repeat(0, 3))
        //         .Concat(Enumerable.Range(0, 10))
        //         .Concat(Enumerable.Repeat(0, 5));
        //
        //     var result = TestIterator(source, 0);
        //
        //     Assert.IsTrue(true);
        // }

        private IEnumerable<int> TrimStartIterator(IEnumerable<int> source, int valueToTrim)
        {
            using (var enumerator = new TrimStartIterator<int>(source, valueToTrim))
            {
                // while (enumerator.SkipStart())
                // {
                //     yield return enumerator.Current;
                // }

                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                    // if (enumerator.Current == 0)
                    // {
                    //     yield return -1;
                    // }
                    // else
                    // {
                    //     yield return enumerator.Current;
                    // }
                }

                // while (enumerator.SkipEnd())
                // {
                //     yield return enumerator.Current;
                // }
            }
        }


        class TrimIteratorTestCaseSource
        {
            private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(Empty)
                        .Returns(Empty)
                        .SetName("EmptySourceTest");
                    yield return new TestCaseData(EnumerableEx.Return(0))
                        .Returns(Empty)
                        .SetName("OneValueToTrimTest");
                    yield return new TestCaseData(Enumerable.Repeat(0, 10))
                        .Returns(Empty)
                        .SetName("OnlyValuesToTrimTest");
                    yield return new TestCaseData(EnumerableEx.Return(1))
                        .Returns(EnumerableEx.Return(1))
                        .SetName("OneValueTest");
                    yield return new TestCaseData(Enumerable.Repeat(1, 10))
                        .Returns(Enumerable.Repeat(1, 10))
                        .SetName("NoValuesToTrimTest");
                    yield return new TestCaseData(new[] {0, 0, 0, 1, 0, 0, 1, 1})
                        .Returns(new[] {1, 0, 0, 1, 1})
                        .SetName("TrimValuesOnlyFromStartTest");
                }
            }
        }
    }
}