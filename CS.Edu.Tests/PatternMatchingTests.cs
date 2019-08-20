using System;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    class PatternMatchingTests
    {
        [Test]
        public void AddTest()
        {
            int? x = 1;
            int? y = 1;
            int? z = null;

            Assert.That(PatternMatching.Add(x, y), Is.EqualTo(2));
            Assert.That(PatternMatching.Add(x, z), Is.EqualTo(1));
            Assert.That(PatternMatching.Add(z, y), Is.EqualTo(1));
            Assert.That(PatternMatching.Add(z, z), Is.EqualTo(0));
        }

        internal int[] AppendExeptNull(int[] array, int value)
        {
            return (array, value) switch
            {
                (null, _) => new int[] { value },
                (int[] acc, 0) when acc.Last() == 0 => array,
                _ => array.Append(value).ToArray()
            };
        }

        [Test]
        public void LINQReduceTest()
        {
            var items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };
            
            var result = items.Aggregate<int, int[]>(null, (acc, cur) => AppendExeptNull(acc, cur));
        }
    }
}
