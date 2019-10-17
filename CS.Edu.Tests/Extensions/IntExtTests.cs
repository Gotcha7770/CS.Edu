using NUnit.Framework;
using CS.Edu.Core.Extensions;
using System.Linq;
using System;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class IntExtTests
    {
        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = false)]
        [TestCase(10, ExpectedResult = true)]
        [TestCase(11, ExpectedResult = false)]
        [TestCase(99, ExpectedResult = false)]
        [TestCase(100, ExpectedResult = true)]
        [TestCase(101, ExpectedResult = false)]
        public bool IsEvenTest(int input)
        {
            return input.IsEven();
        }

        [TestCase(0, ExpectedResult = new long[0])]
        [TestCase(1, ExpectedResult = new[] { 1 })]
        [TestCase(2, ExpectedResult = new[] { 1, 2 })]
        [TestCase(3, ExpectedResult = new[] { 1, 3 })]
        [TestCase(4, ExpectedResult = new[] { 1, 2, 2 })]
        [TestCase(6, ExpectedResult = new[] { 1, 2, 3 })]
        [TestCase(12, ExpectedResult = new[] { 1, 2, 2, 3 })]
        [TestCase(120, ExpectedResult = new[] { 1, 2, 2, 2, 3, 5 })]
        public long[] FactorizeTest(long input)
        {
            return input.Factorize().ToArray();
        }

        [TestCase(0, ExpectedResult = false)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = true)]
        [TestCase(4, ExpectedResult = false)]
        [TestCase(5, ExpectedResult = true)]
        [TestCase(6, ExpectedResult = false)]
        [TestCase(7, ExpectedResult = true)]
        [TestCase(11, ExpectedResult = true)]
        [TestCase(13, ExpectedResult = true)]
        [TestCase(17, ExpectedResult = true)]
        [TestCase(19, ExpectedResult = true)]
        [TestCase(21, ExpectedResult = false)]
        [TestCase(23, ExpectedResult = true)]
        [TestCase(101, ExpectedResult = true)]
        [TestCase(1009, ExpectedResult = true)]
        [TestCase(10007, ExpectedResult = true)]
        public bool IsPrimeTest(long input)
        {
            return input.IsPrime();
        }
    }
}
