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

        [Test]
        public void CycleReduceTest()
        {
            var items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };

            List<int> result = new List<int>();

            bool skippingZeros = false;
            foreach (var item in items)
            {
                if (item != 0)
                {
                    skippingZeros = false;
                    result.Add(item);
                }
                else if (!skippingZeros)
                {
                    skippingZeros = true;
                    result.Add(item);
                }
            }
        }

        [Test]
        public void LINQReduceTest()
        {
            var items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1, 0, 0 }; 
            Func<int, bool> nonZero = x => x != 0;

            static IEnumerable<TSource> BufferIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> constraint) //Window
            {
                IEnumerable<TSource> left = source;
                while (left.Any())
                {
                    foreach (TSource item in left.TakeWhile(constraint))
                    {
                        yield return item;
                    }

                    left = left.SkipWhile(constraint);
                    yield return left.FirstOrDefault();

                    left = left.SkipWhile(x => !constraint(x));
                }
            }

            List<int> result = BufferIterator(items, nonZero).ToList();
        }

        [Test]
        public void SpiltTest()
        {
            var items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1, 0, 0 };
            Relation<int> relation = new Relation<int>((x, y) => (x, y) switch {
                (0, 0) => true,
                (0, _) => false,
                (_, 0) => false,
                _ => true
            });

            List<int> result = items.Split(relation)
                .Select(x => x.All(y => y == 0) ? Enumerable.Repeat(x.First(), 1) : x)
                .SelectMany(x => x)
                .ToList();
        }
    }
}
