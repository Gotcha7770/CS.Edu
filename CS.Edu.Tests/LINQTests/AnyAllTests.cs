using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class AnyAllTests
    {
        [Test]
        public void All_WithoutItems_ReturnsTrue()
        {
            var result = Enumerable.Empty<int>().All(x => x % 2 == 0);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Any_WithoutItems_ReturnsFalse()
        {
            var result = Enumerable.Empty<int>().Any(x => x % 2 != 0);

            Assert.AreEqual(false, result);
        }
    }
}