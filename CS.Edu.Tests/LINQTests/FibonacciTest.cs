using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class FibonacciTest
    {
        public static int Fibonacci(int n)
        {
            return Fibonacci(0, 1, n);
        }

        static int Fibonacci(int a, int b, int number)
        {
            return number > 1 ? Fibonacci(b, b + a, --number) : b;
        }

        static IEnumerable<int> FibonacciIterator()
        {
            for (int x = 0, y = 1; ; y = x + y, x = y - x)
            {
                yield return y;
            }
        }

        [Test]
        public void Test()
        {
            int tenth1 = Fibonacci(10);
            int tenth2 = FibonacciIterator().ElementAt(9);
            int tenth3 = Enumerable.Range(0, int.MaxValue)
                                   .Scan((X: 0, Y: 1), (acc, curr) => (acc.Y, acc.X + acc.Y))
                                   .ElementAt(9).X;

            Assert.That(tenth1, Is.EqualTo(55));
            Assert.That(tenth2, Is.EqualTo(55));
            Assert.That(tenth3, Is.EqualTo(55));
        }
    }
}