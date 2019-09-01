using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Tests.Extensions
{
    [TestFixture]
    public class SplitTests2
    {
        public IEnumerable<IEnumerable<TSource>> Split2<TSource>(IEnumerable<TSource> source, Relation<TSource> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            int countToSkip = 0;
            while (source.Skip(countToSkip).Any())
            {
                yield return TakeIterator(source, countToSkip, relation);                

                countToSkip += CounterIterator(source, countToSkip, relation);
            }
        }

        static IEnumerable<TSource> TakeIterator<TSource>(IEnumerable<TSource> source, int countToSkip, Relation<TSource> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext()) countToSkip--;
                
                if (!enumerator.MoveNext())
                    yield break;

                TSource prev = enumerator.Current;
                yield return prev;

                while (enumerator.MoveNext())
                {
                    if (!relation(prev, enumerator.Current))
                        break;

                    prev = enumerator.Current;
                    yield return prev;
                }
            }
        }

        static int CounterIterator<TSource>(IEnumerable<TSource> source, int countToSkip, Relation<TSource> relation)
        {            
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext()) countToSkip--;

                if (!enumerator.MoveNext())
                    return 0;

                int result = 1;
                TSource prev = enumerator.Current;

                while(enumerator.MoveNext() && relation(prev, enumerator.Current))
                {
                    result++;
                    prev = enumerator.Current;
                }

                return result;
            }            
        }

        [Test]
        public void Split_Empty_ReturnsEmpty()
        {
            var items = Enumerable.Empty<int>();
            Relation<int> lessThan = (x, y) => x < y;

            var result = Split2(items, lessThan).ToArray();

            Assert.That(result, Is.EqualTo(Array.Empty<int>()));
        }

        [Test]
        public void SplitFirstDiminsion_OneElement_ReturnsOneArrayWithThatElement()
        {
            var items = new int[] { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var tmp = Split2(items, lessThan);
            var result = tmp.ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void SplitSecondDimension_OneElement_ReturnsOneArrayWithThatElement()
        {
            var items = new int[] { 1 };
            Relation<int> lessThan = (x, y) => x < y;

            var tmp = Split2(items, lessThan);
            var result = Split2(items, lessThan)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void SplitFirstDiminsion_WhilePrevLessThenNext_Returns2Groups()
        {
            var items = new int[] { 1, 2, 3, 2, 3 };
            Relation<int> lessThan = (x, y) => x < y;

            var result = Split2(items, lessThan)
                .ToArray();            

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2, 3 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 2, 3 }));
        }

        [Test]
        public void SplitSecondDiminsion_WhilePrevLessThenNext_Returns2Groups()
        {
            var items = new int[] { 1, 2, 3, 2, 3 };
            Relation<int> lessThan = (x, y) => x < y;
            
            var result = Split2(items, lessThan)
                .Select(x => x.ToArray())
                .ToArray();

            Assert.That(result.Length, Is.EqualTo(2));
            Assert.That(result[0], Is.EqualTo(new int[] { 1, 2, 3 }));
            Assert.That(result[1], Is.EqualTo(new int[] { 2, 3 }));
        }
    }
}