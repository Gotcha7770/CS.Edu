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
        public long GetNthPrime(int n)
        {
            return PrimesGenerator.GetPrimes().ElementAt(n);
        }
    }
}