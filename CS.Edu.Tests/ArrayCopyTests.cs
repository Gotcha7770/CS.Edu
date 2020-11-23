using CS.Edu.Core.Extensions;
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

            _source.CopyPart(target, 0, 0, 0);

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

            _source.CopyPart(target, 1, 1, 0);

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

            _source.CopyPart(target, 2, 2, 0);

            Assert.AreEqual(standard, target);
        }
    }
}