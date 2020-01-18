using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class LongExtTests
    {
        [Test]
        public void LongRange_Empty()
        {
            IEnumerable<long> items = LongExt.Range(0, 0);

            Assert.That(items.IsEmpty);
        }

        [Test]
        public void LongRange_Single()
        {
            IEnumerable<long> items = LongExt.Range(0, 1);

            Assert.That(items.Single(), Is.EqualTo(1));
        }

        [Test]
        public void LongRange_Several()
        {
            IEnumerable<long> items = LongExt.Range(0, 10);

            Assert.That(items, Is.EqualTo(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
        }

        [Test]
        public void LongRange_SeveralWithStep()
        {
            IEnumerable<long> items = LongExt.Range(0, 10, 10);

            Assert.That(items, Is.EqualTo(new[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 }));
        }

        [Test]
        public void LongRange_OutOfRange()
        {
            IEnumerable<long> items = LongExt.Range(long.MaxValue - 1, 1);

            Assert.That(items.Last(), Is.EqualTo(long.MaxValue - 1));
        }
    }
}