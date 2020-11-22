using System;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class ArrayCopyTests
    {
        private readonly int[,] _source =
        {
            {0, 1, 2, 3},
            {4, 5, 6, 7},
            {8, 9, 10, 11},
            {12, 13, 14, 15}
        };

        [Test]
        public void TwoDimensialArrayCopy_LeftTopCorner()
        {
            var target = new int [2, 2];
            int[,] standard =
            {
                {0, 1},
                {4, 5}
            };

            _source.CopyPart(target, 0, 0);

            Assert.AreEqual(standard, target);
        }

        [Test]
        public void TwoDimensialArrayCopy_Center()
        {
            var target = new int [2, 2];
            int[,] standard =
            {
                {5, 6},
                {9, 10}
            };

            _source.CopyPart(target, 1, 1);

            Assert.AreEqual(standard, target);
        }

        [Test]
        public void TwoDimensialArrayCopy_RightBottomCorner()
        {
            var target = new int [2, 2];
            int[,] standard =
            {
                {10, 11},
                {14, 15}
            };

            _source.CopyPart(target, 2, 2);

            Assert.AreEqual(standard, target);
        }
    }

    public static class ArrayExtensions
    {
        public static void CopyPart<T>(this T[,] source, T[,] target, int row, int column)
        {
            Implementation1<T>(source, target, row, column);
        }

        private static void Implementation1<T>(T[,] source, T[,] target, int row, int column)
        {
            if(source.GetLength(0) < target.GetLength(0)
               || source.GetLength(1) < target.GetLength(1))
                throw new ArgumentException($"dimensions of {nameof(source)} should be grater then dimensions of {nameof(target)}");

            for (int i = 0; i < target.GetLength(0); i++)
            for (int j = 0; j < target.GetLength(1); j++)
            {
                target[i, j] = source[i + row, j + column];
            }
        }

        //private static void Implementation2<T>(T[,] source, T[,] target, int row, int column){}
        //Array.Copy()

        //private static void Implementation3<T>(T[,] source, T[,] target, int row, int column){}
        //Buffer.BlockCopy(source, 5, target, 0, 4 * sizeof(int));
    }
}