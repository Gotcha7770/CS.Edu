using NUnit.Framework;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    enum TestEnum
    {
        One,
        Two,
        Three
    }

    [TestFixture]
    public class EnumExtTests
    {
        [Test]
        public void GetValuesTest()
        {
            var values = EnumExt.GetValues<TestEnum>();

            CollectionAssert.AreEqual(values, new [] {TestEnum.One, TestEnum.Two, TestEnum.Three});
        }
    }
}