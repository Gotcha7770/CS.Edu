using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.MathExt;
using NUnit.Framework;

namespace CS.Edu.Tests.MathExt
{
    [TestFixture]
    public class NumberTests
    {
        [TestCase(2)]
        [TestCase(8)]
        [TestCase(10)]
        [TestCase(16)]
        public void DigitsTest_Zero_ReturnsZero(int @base)
        {
            int[] result = Number.Digits(0, @base);
            Assert.That(result, Is.EqualTo(new[] { 0 }));
        }

        [Test]
        public void DigitsTest_BaseIs2()
        {
            int[] result = Number.Digits(0b1011, 2);
            Assert.That(result, Is.EqualTo(new[] { 1, 1, 0, 1 }));
        }

        [Test]
        public void DigitsTest_BaseIs10()
        {
            int[] result = Number.Digits(123, 10);
            Assert.That(result, Is.EqualTo(new[] { 3, 2, 1 }));
        }

        [Test]
        public void ReductionTest()
        {
            var array = new[] { 3, 2, 1 };

            long number = array.Select((x,i) => x * 10.Power(i)).Sum();

            Assert.That(number, Is.EqualTo(123));
        }
    }
}