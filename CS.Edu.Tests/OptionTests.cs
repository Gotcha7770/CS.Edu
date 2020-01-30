using System.Collections;
using CS.Edu.Core;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void CastTest()
        {
            Assert.DoesNotThrow(() =>
            {
                Option<int> one = Option.None;
            });
        }

        [TestCaseSource(typeof(EqualsTestsDataSource), "TestCases")]
        public bool EqualsTest(Option<int> one, Option<int> other)
        {
            return Equals(one, other);
        }

        internal class EqualsTestsDataSource
        {
            public static IEnumerable TestCases
            {
                get
                {
                    Option<int> none = Option.None;
                    var option = Option.Some(0);

                    yield return new TestCaseData(none, none).Returns(true);
                    yield return new TestCaseData(none, option).Returns(false);
                    yield return new TestCaseData(option, option).Returns(true);
                    yield return new TestCaseData(option, Option.Some(0)).Returns(true);
                    yield return new TestCaseData(option, Option.Some(1)).Returns(false);
                }
            }
        }
    }
}
