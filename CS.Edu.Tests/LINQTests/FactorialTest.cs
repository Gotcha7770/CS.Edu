using CS.Edu.Core.Math;
using NUnit.Framework;
using System.Linq;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class FactorialTest
    {
        [Test]
        public void Test()
        {
            int fact1 = Factorial.Recursive(5);
            int fact2 = Factorial.Iterator().ElementAt(5);
            int fact3 = Enumerable.Range(0, 6).Aggregate((acc, cur) => acc == 0 ? 1 : acc * cur);

            Assert.That(fact1, Is.EqualTo(120));
            Assert.That(fact2, Is.EqualTo(120));
            Assert.That(fact3, Is.EqualTo(120));
        }
    }
}