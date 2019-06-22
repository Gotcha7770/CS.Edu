using NUnit.Framework;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class IntExtTests
    {
        [Test]
        public void IsEven_0_ReturnsTrue()
        {
            Assert.That(0.IsEven(), Is.True);
        }

        [Test]
        public void IsEven_Odd_ReturnsFalse()
        {
            Assert.That(1.IsEven(), Is.False);
            Assert.That(3.IsEven(), Is.False);
            Assert.That(11.IsEven(), Is.False);
        }

        [Test]
        public void IsEven_Even_ReturnsTrue()
        {
            Assert.That(2.IsEven(), Is.True);
            Assert.That(4.IsEven(), Is.True);
            Assert.That(12.IsEven(), Is.True);
        }
    }
}
