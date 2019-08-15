using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
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

            Assert.That(PatternMatching.Add(x, y), Is.EqualTo(2));
            Assert.That(PatternMatching.Add(x, z), Is.EqualTo(1));
            Assert.That(PatternMatching.Add(z, y), Is.EqualTo(1));
            Assert.That(PatternMatching.Add(z, z), Is.EqualTo(0));
        }

        [Test]
        public void TestMethod() //придумать лучшее воплощение
        {
            var items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1};

            List<int> result = new List<int>();

            bool isZero = false;
            foreach (var item in items)
            {
                if (item != 0)
                {
                    isZero = false;
                    result.Add(item);
                }
                else if (!isZero)
                {
                    isZero = true;
                    result.Add(item);
                }
            }
        }
    }
}
