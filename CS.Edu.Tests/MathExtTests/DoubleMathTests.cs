using System;
using NUnit.Framework;

namespace CS.Edu.Tests.MathExtTests
{
    [TestFixture]
    public class DoubleMathTests
    {
        [Test]
        public void Test4()
        {
            Assert.AreEqual(double.NegativeInfinity, 2 * double.NegativeInfinity);
            Assert.AreEqual(double.NegativeInfinity, double.NegativeInfinity * 2);
            Assert.AreEqual(double.PositiveInfinity, double.NegativeInfinity * double.NegativeInfinity);
            Assert.AreEqual(double.PositiveInfinity, double.PositiveInfinity * double.PositiveInfinity);
            Assert.AreEqual(double.NaN, double.NaN * double.NegativeInfinity);
            Assert.AreEqual(double.NaN, double.NegativeInfinity * double.NaN);
            Assert.AreEqual(double.NaN, 2 * double.NaN);
            Assert.AreEqual(double.NaN, double.NaN * 2);
        }
    }
}