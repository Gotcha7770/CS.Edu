using System;
using System.Collections;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class RangeTests
    {
        [TestCaseSource(typeof(EqualsTestsDataSource), "TestCases")]
        public bool EqualsTest(Range<int> one, Range<int> other)
        {
            return Equals(one, other);
        }

        internal class EqualsTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    var range = new Range<int>(5, 9);

                    yield return new TestCaseData(range, null).Returns(false);
                    yield return new TestCaseData(range, Range<int>.Default).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(4, 9)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(6, 9)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(5, 8)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(5, 10)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(4, 10)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(6, 8)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(5, 9)).Returns(true);
                    yield return new TestCaseData(range, range).Returns(true);
                }
            }
        }

        [TestCase(4, ExpectedResult = false)]
        [TestCase(5, ExpectedResult = true)]
        [TestCase(7, ExpectedResult = true)]
        [TestCase(9, ExpectedResult = true)]
        [TestCase(10, ExpectedResult = false)]
        public bool ContainsValueTests(int value)
        {
            var range = new Range<int>(5, 9);

            return range.Contains(value);
        }

        [TestCaseSource(typeof(ContainsTestsDataSource), "TestCases")]
        public bool ContainsRangeTests(Range<int> one, Range<int> other)
        {
            return one.Contains(other);
        }

        internal class ContainsTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    var range = new Range<int>(5, 9);

                    yield return new TestCaseData(range, new Range<int>(5, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(5, 8)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(6, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(6, 8)).Returns(true);

                    yield return new TestCaseData(range, new Range<int>(0, 4)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(0, 5)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(0, 6)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(0, 8)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(0, 9)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(0, 10)).Returns(false);

                    yield return new TestCaseData(range, new Range<int>(4, 11)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(5, 11)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(6, 11)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(8, 11)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(9, 11)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(10, 11)).Returns(false);
                }
            }
        }

        [TestCaseSource(typeof(IntersectsTestsDataSource), "TestCases")]
        public bool IntersectsTests(Range<int> one, Range<int> other)
        {
            return one.Intersects(other);
        }

        internal class IntersectsTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    var range = new Range<int>(5, 9);

                    yield return new TestCaseData(range, new Range<int>(0, 5)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(0, 6)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(0, 8)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(0, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(0, 10)).Returns(true);

                    yield return new TestCaseData(range, new Range<int>(4, 8)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(4, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(4, 10)).Returns(true);

                    yield return new TestCaseData(range, new Range<int>(5, 8)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(5, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(5, 10)).Returns(true);

                    yield return new TestCaseData(range, new Range<int>(6, 8)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(6, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(6, 10)).Returns(true);

                    yield return new TestCaseData(range, new Range<int>(8, 9)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(8, 10)).Returns(true);
                    yield return new TestCaseData(range, new Range<int>(9, 10)).Returns(true);

                    yield return new TestCaseData(range, new Range<int>(0, 4)).Returns(false);
                    yield return new TestCaseData(range, new Range<int>(10, 15)).Returns(false);
                }
            }
        }

        [TestCaseSource(typeof(IntersectionTestsDataSource), "TestCases")]
        public Range<int> IntersectionTests(Range<int> one, Range<int> other)
        {
            return one.Intersection(other);
        }

        internal class IntersectionTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    var range = new Range<int>(5, 9);

                    yield return new TestCaseData(range, new Range<int>(0, 5)).Returns(new Range<int>(5, 5));
                    yield return new TestCaseData(range, new Range<int>(0, 6)).Returns(new Range<int>(5, 6));
                    yield return new TestCaseData(range, new Range<int>(0, 8)).Returns(new Range<int>(5, 8));
                    yield return new TestCaseData(range, new Range<int>(0, 9)).Returns(new Range<int>(5, 9));
                    yield return new TestCaseData(range, new Range<int>(0, 10)).Returns(new Range<int>(5, 9));

                    yield return new TestCaseData(range, new Range<int>(4, 8)).Returns(new Range<int>(5, 8));
                    yield return new TestCaseData(range, new Range<int>(4, 9)).Returns(new Range<int>(5, 9));
                    yield return new TestCaseData(range, new Range<int>(4, 10)).Returns(new Range<int>(5, 9));

                    yield return new TestCaseData(range, new Range<int>(5, 8)).Returns(new Range<int>(5, 8));
                    yield return new TestCaseData(range, new Range<int>(5, 9)).Returns(new Range<int>(5, 9));
                    yield return new TestCaseData(range, new Range<int>(5, 10)).Returns(new Range<int>(5, 9));

                    yield return new TestCaseData(range, new Range<int>(6, 8)).Returns(new Range<int>(6, 8));
                    yield return new TestCaseData(range, new Range<int>(6, 9)).Returns(new Range<int>(6, 9));
                    yield return new TestCaseData(range, new Range<int>(6, 10)).Returns(new Range<int>(6, 9));

                    yield return new TestCaseData(range, new Range<int>(8, 9)).Returns(new Range<int>(8, 9));
                    yield return new TestCaseData(range, new Range<int>(8, 10)).Returns(new Range<int>(8, 9));
                    yield return new TestCaseData(range, new Range<int>(9, 10)).Returns(new Range<int>(9, 9));

                    yield return new TestCaseData(range, new Range<int>(0, 4)).Returns(Range<int>.Default);
                    yield return new TestCaseData(range, new Range<int>(10, 15)).Returns(Range<int>.Default);
                }
            }
        }

        [TestCaseSource(typeof(SubstructionTestsDataSource), "TestCases")]
        public Range<int>[] SubstructionTests(Range<int> one, Range<int> other)
        {
            return one.Substruct(other);
        }

        [TestCaseSource(typeof(SubstructionTestsDataSource), "TestCases")]
        public Range<int>[] SubstructionTest2(Range<int> one, Range<int> other)
        {
            Func<Range<int>, Range<int>, Range<int>[]> func = (x, y) => 
            {
                var tmp1 = Operators.Min<int>(x.Min, y.Min);
                var tmp2 = Operators.Min<int>(x.Min, y.Max);
                var tmp3 = Operators.Min<int>(x.Max, y.Min);
                var tmp4 = Operators.Min<int>(x.Max, y.Max);
                
                var tmp5 = Operators.Max<int>(x.Min, y.Min);
                var tmp6 = Operators.Max<int>(x.Min, y.Max);
                var tmp7 = Operators.Max<int>(x.Max, y.Min);
                var tmp8 = Operators.Max<int>(x.Max, y.Max);

                var result = new [] 
                {
                    new Range<int>(tmp1, tmp3),
                    new Range<int>(tmp6, tmp8)                    
                };

                return result;
            };

            return func(one, other).Where(x => !x.IsEmpty).ToArray();
        }

        internal class SubstructionTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    var range = new Range<int>(5, 9);

                    yield return new TestCaseData(range, new Range<int>(0, 4))
                        .Returns(new[] { new Range<int>(5, 9) });
                    yield return new TestCaseData(range, new Range<int>(0, 5))
                        .Returns(new[] { new Range<int>(5, 9) });
                    yield return new TestCaseData(range, new Range<int>(0, 6))
                        .Returns(new[] { new Range<int>(6, 9) });
                    yield return new TestCaseData(range, new Range<int>(0, 8))
                        .Returns(new[] { new Range<int>(8, 9) });
                    
                    yield return new TestCaseData(range, new Range<int>(0, 9))
                        .Returns(Array.Empty<Range<int>>());
                    yield return new TestCaseData(range, new Range<int>(0, 10))
                        .Returns(Array.Empty<Range<int>>());
                    yield return new TestCaseData(range, new Range<int>(5, 9))
                        .Returns(Array.Empty<Range<int>>());
                    yield return new TestCaseData(range, new Range<int>(5, 10))
                        .Returns(Array.Empty<Range<int>>());

                    yield return new TestCaseData(range, new Range<int>(6, 8))
                        .Returns(new[] { new Range<int>(5, 6), new Range<int>(8, 9) });
                    yield return new TestCaseData(range, new Range<int>(7, 7))
                        .Returns(new[] { new Range<int>(5, 7), new Range<int>(7, 9) });
                    
                    yield return new TestCaseData(range, new Range<int>(6, 10))
                        .Returns(new[] { new Range<int>(5, 6) });
                    yield return new TestCaseData(range, new Range<int>(8, 10))
                        .Returns(new[] { new Range<int>(5, 8) });
                    yield return new TestCaseData(range, new Range<int>(9, 10))
                        .Returns(new[] { new Range<int>(5, 9) });
                    yield return new TestCaseData(range, new Range<int>(10, 11))
                        .Returns(new[] { new Range<int>(5, 9) });
                }
            }
        }
    }
}