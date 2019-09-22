using CS.Edu.Core.Extensions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class FactorialTest
    {
        public static int Factorial(int n)
        {
            if (n == 0)
                return 1;

            return n * Factorial(n - 1);
        }

        public static IEnumerable<int> FactorialIterator()
        {
            int acc = 1;
            for (int i = 1; ; i++)
            {
                yield return acc;
                acc *= i;
            }
        }

        [Test]
        public void Test()
        {
            int fact1 = Factorial(5);
            int fact2 = FactorialIterator().Take(6).Last();
            int fact3 = Enumerable.Range(0, 6).Aggregate((acc, cur) => acc == 0 ? 1 : acc * cur);

            Assert.That(fact1, Is.EqualTo(120));
            Assert.That(fact2, Is.EqualTo(120));
            Assert.That(fact3, Is.EqualTo(120));
        }
    }
}