using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests.LINQTests
{
    [TestFixture]
    public class CircularBufferTests
    {
        [Test]
        public void CircularBufferWithLINQ()
        {
            var items = new[] { 0, 1, 2 };
            var buffer = items.Repeat();

            CollectionAssert.AreEqual(new [] {0, 1, 2, 0, 1}, buffer.Take(5));
        }
    }
}