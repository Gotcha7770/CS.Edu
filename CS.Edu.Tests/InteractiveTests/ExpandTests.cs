
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.ExpandTests
{
    [TestFixture]
    public class ExpandTests
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
            int x = 1;
            yield return x;

            int y = 1;
            yield return y;

            while (true)
            {
                y = x + y;
                x = y - x;

                yield return y;
            }
        }

        [Test]
        public void FibonacciTest()
        {
            int tenth1 = Fibonacci(10);
            int tenth2 = FibonacciIterator().Take(10).Last();
        }

        public static int Factorial(int n)
        {
            if (n == 0)
                return 1;

            return n * Factorial(n - 1);
        }

        public static IEnumerable<int> FactorialIterator()
        {
            yield return 1;

            int x = 1;
            int acc = 1;
            while (true)
            {
                yield return acc;
                x++;
                acc *= x;
            }
        }

        [Test]
        public void FactorialTest()
        {
            int fact1 = Factorial(5);
            int fect2 = FactorialIterator().Take(6).Last();
        }

        [Test]
        public void Test1()
        {
            // f(x)
            // 1 -> 1 [2]
            // 2 -> 2 [3]
            // 3 -> 6 [4]
            var seed = new[] { 1 };
            var result = seed.Expand(x => EnumerableEx.Return(x * (x + 1))).Take(5);
        }

        [Test]
        public void Test2()
        {
            //var result = EnumerableEx.Generate(0, x => true, x => x++, x => )
        }
    }
}