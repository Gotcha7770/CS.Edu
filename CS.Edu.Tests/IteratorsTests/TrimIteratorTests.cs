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
        private readonly TrimIteratorStateMachine<int> _state = new TrimIteratorStateMachine<int>(x => x == 0);

        [SetUp]
        public void Setup()
        {
            _state.Reset();
        }

        [TestCaseSource(typeof(TrimIteratorTestCaseSource), nameof(TrimIteratorTestCaseSource.TrimStartTestCases))]
        public IEnumerable<int> TrimStartIteratorTests(IEnumerable<int> source)
        {
            return TrimStartIterator(source);
        }

        [TestCaseSource(typeof(TrimIteratorTestCaseSource), nameof(TrimIteratorTestCaseSource.ReplaceMiddleTestCases))]
        public IEnumerable<int> ReplaceOnlyInTheMiddleIteratorTests(IEnumerable<int> source)
        {
            return TrimStartIterator(source);
        }

        [TestCaseSource(typeof(TrimIteratorTestCaseSource), nameof(TrimIteratorTestCaseSource.TrimEndTestCases))]
        public IEnumerable<int> TrimEndIteratorTests(IEnumerable<int> source)
        {
            return TrimStartIterator(source);
        }

        private IEnumerable<int> TrimStartIterator(IEnumerable<int> source)
        {
            using (var enumerator = new TrimStartIterator<int>(source, _state))
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }

                while (enumerator.SkipEnd())
                {
                    yield return enumerator.Current;
                }
            }
        }

        private IEnumerable<int> ReplaceOnlyInTheMiddleIterator(IEnumerable<int> source)
        {
            using (var enumerator = new TrimStartIterator<int>(source, _state))
            {
                while (enumerator.SkipStart())
                {
                    yield return enumerator.Current;
                }

                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == 0)
                    {
                        yield return -1;
                    }
                    else
                    {
                        yield return enumerator.Current;
                    }
                }

                while (enumerator.SkipEnd())
                {
                    yield return enumerator.Current;
                }
            }
        }

        private IEnumerable<int> TrimEndIterator(IEnumerable<int> source)
        {
            using (var enumerator = new TrimStartIterator<int>(source, _state))
            {
                while (enumerator.SkipStart())
                {
                    yield return enumerator.Current;
                }

                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
        }


        class TrimIteratorTestCaseSource
        {
            private static readonly IEnumerable<int> Empty = Enumerable.Empty<int>();

            public static IEnumerable TrimStartTestCases
            {
                get
                {
                    yield return new TestCaseData(Empty)
                        .Returns(Empty)
                        .SetCategory("TrimStart")
                        .SetName("EmptySourceTest");
                    yield return new TestCaseData(EnumerableEx.Return(0))
                        .Returns(Empty)
                        .SetCategory("TrimStart")
                        .SetName("OneValueToTrimTest");
                    yield return new TestCaseData(Enumerable.Repeat(0, 10))
                        .Returns(Empty)
                        .SetCategory("TrimStart")
                        .SetName("OnlyValuesToTrimTest");
                    yield return new TestCaseData(EnumerableEx.Return(1))
                        .Returns(EnumerableEx.Return(1))
                        .SetCategory("TrimStart")
                        .SetName("OneValueTest");
                    yield return new TestCaseData(Enumerable.Repeat(1, 10))
                        .Returns(Enumerable.Repeat(1, 10))
                        .SetCategory("TrimStart")
                        .SetName("NoValuesToTrimTest");
                    yield return new TestCaseData(new[] {0, 0, 0, 1, 0, 0, 1, 1})
                        .Returns(new[] {1, 0, 0, 1, 1})
                        .SetCategory("TrimStart")
                        .SetName("TrimValuesOnlyFromStartTest");
                }
            }

            public static IEnumerable ReplaceMiddleTestCases
            {
                get
                {
                    yield return new TestCaseData(Empty)
                        .Returns(Empty)
                        .SetCategory("ReplaceMiddle")
                        .SetName("EmptySourceTest");
                }
            }

            public static IEnumerable TrimEndTestCases
            {
                get
                {
                    yield return new TestCaseData(Empty)
                        .Returns(Empty)
                        .SetCategory("TrimEnd")
                        .SetName("EmptySourceTest");
                }
            }
        }
    }
}