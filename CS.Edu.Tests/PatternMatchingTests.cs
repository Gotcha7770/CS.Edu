using CS.Edu.Core;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    class PatternMatchingTests
    {
        [Test]
        public void AddTest()
        {
            int? x = 1;
            int? y = 1;
            int? z = null;

            Assert.That(PatternMathcing.Add(x, y), Is.EqualTo(2));
            Assert.That(PatternMathcing.Add(x, z), Is.EqualTo(1));
            Assert.That(PatternMathcing.Add(z, y), Is.EqualTo(1));
            Assert.That(PatternMathcing.Add(z, z), Is.EqualTo(0));
        }
    }
}
