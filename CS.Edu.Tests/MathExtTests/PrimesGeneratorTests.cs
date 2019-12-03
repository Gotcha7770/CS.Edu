using System;
using System.Linq;
using CS.Edu.Core.MathExt;
using NUnit.Framework;

namespace CS.Edu.Tests.MathExt
{
    [TestFixture]
    public class PrimesGeneratorTests
    {
        [Test]
        public void FirstPrimeIs2()
        {
            long firstPrime = PrimesGenerator.GetPrimes().First();
            Assert.That(firstPrime, Is.EqualTo(2));
        }

        [Test]
        public void TwoPrimesIterators_SeparatedSequences()
        {
            long one = PrimesGenerator.GetPrimes().First();
            long other = PrimesGenerator.GetPrimes().First();

            Assert.That(one, Is.EqualTo(other));
        }

        [TestCase(0, ExpectedResult = 2)]
        [TestCase(1, ExpectedResult = 3)]
        [TestCase(2, ExpectedResult = 5)]
        [TestCase(3, ExpectedResult = 7)]
        [TestCase(4, ExpectedResult = 11)]
        [TestCase(5, ExpectedResult = 13)]
        [TestCase(6, ExpectedResult = 17)]
        [TestCase(7, ExpectedResult = 19)]
        [TestCase(28, ExpectedResult = 109)]
        [TestCase(49, ExpectedResult = 229)]
        [TestCase(70, ExpectedResult = 353)]
        [TestCase(91, ExpectedResult = 479)]
        [TestCase(112, ExpectedResult = 617)]
        public long GetNthPrime(int n)
        {
            return PrimesGenerator.GetPrimes().ElementAt(n);
        }
    }
}