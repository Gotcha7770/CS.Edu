using System.Collections;
using CS.Edu.Core;
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
                    yield return new TestCaseData(new Range<int>(0, 10), null).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 10), new Range<int>(0, 9)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 10), new Range<int>(1, 10)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 10), new Range<int>(1, 9)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 10), new Range<int>(0, 10)).Returns(true);
                    var range = new Range<int>(0, 10);
                    yield return new TestCaseData(range, range).Returns(true);
                }
            }
        }

        [TestCaseSource(typeof(ContainsTestsDataSource), "TestCases")]
        public bool ContainsTests(Range<int> other)
        {
            var range = new Range<int>(10, 100);

            return range.Contains(other);
        }

        internal class ContainsTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new Range<int>(10, 100)).Returns(true);
                    yield return new TestCaseData(new Range<int>(10, 99)).Returns(true);
                    yield return new TestCaseData(new Range<int>(11, 100)).Returns(true);
                    yield return new TestCaseData(new Range<int>(11, 99)).Returns(true);
                    yield return new TestCaseData(new Range<int>(0, 9)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 10)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 11)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 100)).Returns(false);
                    yield return new TestCaseData(new Range<int>(0, 101)).Returns(false);
                    yield return new TestCaseData(new Range<int>(10, 101)).Returns(false);
                    yield return new TestCaseData(new Range<int>(11, 101)).Returns(false);
                    yield return new TestCaseData(new Range<int>(100, 101)).Returns(false);
                }
            }
        }

        [TestCaseSource(typeof(IntersectsTestsDataSource), "TestCases")]
        public bool IntersectsTests(Range<int> other)
        {
            var range = new Range<int>(10, 100);

            return range.Intersects(other);
        }

        internal class IntersectsTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new Range<int>(0, 10)).Returns(true);
                    yield return new TestCaseData(new Range<int>(0, 11)).Returns(true);
                    yield return new TestCaseData(new Range<int>(0, 100)).Returns(true);
                    yield return new TestCaseData(new Range<int>(0, 101)).Returns(true);
                    yield return new TestCaseData(new Range<int>(10, 99)).Returns(true);
                    yield return new TestCaseData(new Range<int>(10, 100)).Returns(true);
                    yield return new TestCaseData(new Range<int>(10, 101)).Returns(true);
                    yield return new TestCaseData(new Range<int>(11, 99)).Returns(true);
                    yield return new TestCaseData(new Range<int>(11, 100)).Returns(true);
                    yield return new TestCaseData(new Range<int>(11, 101)).Returns(true);
                    yield return new TestCaseData(new Range<int>(100, 101)).Returns(true);
                    yield return new TestCaseData(new Range<int>(0, 9)).Returns(false);
                    yield return new TestCaseData(new Range<int>(101, 110)).Returns(false);
                }
            }
        }

        [TestCaseSource(typeof(IntersectionTestsDataSource), "TestCases")]
        public Range<int> IntersectionTests(Range<int> other)
        {
            var range = new Range<int>(10, 100);

            return range.Intersection(other);
        }

        internal class IntersectionTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new Range<int>(0, 9)).Returns(Range.Empty<int>());
                }
            }
        }
    }
}