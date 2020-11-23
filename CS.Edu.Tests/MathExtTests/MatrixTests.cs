using System;
using System.Linq;
using CS.Edu.Core.MathExt;
using NUnit.Framework;

namespace CS.Edu.Tests.MathExt
{
    [TestFixture]
    public class MatrixTests
    {
        [Test]
        public void CreateJagged()
        {
            double[][] mtrx = Enumerable.Range(0, 4)
                .Select(c => Enumerable.Range(0, 1000).Select(r => r * Math.Pow(10, c)).ToArray())
                .ToArray();

            Assert.That(mtrx.Length, Is.EqualTo(4));
            Assert.That(mtrx[0].Length, Is.EqualTo(1000));
        }

        [Test]
        public void ConvertTo2D_Jagged_returns2D()
        {
            double[][] mtrx = Enumerable.Range(0, 4)
                .Select(c => Enumerable.Range(0, 1000).Select(r => r * Math.Pow(10, c)).ToArray())
                .ToArray();

            double[,] result = mtrx.To2D();

            Assert.That(result.Rank, Is.EqualTo(2));
            Assert.That(result.GetLength(0), Is.EqualTo(4));
            Assert.That(result.GetLength(1), Is.EqualTo(1000));
        }
    }
}
