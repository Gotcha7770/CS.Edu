using NUnit.Framework;
using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using System.Collections.Generic;
using Range = System.Range;
using System.Diagnostics;

namespace CS.Edu.Tests
{
    public enum Direction
    {
        Ascending,
        Descending
    }

    [TestFixture]
    public class Temptests
    {
        Relation<Indexed<int>, Indexed<int>, Indexed<int>> isMonotone = (x, y, z) =>
            x.Value < y.Value ? y.Value < z.Value : y.Value > z.Value;

        Relation<Indexed<int>, Indexed<int>, Direction> isDirectionChanged = (x, y, dir) =>
            dir == Direction.Ascending ? x.Value > y.Value : x.Value < y.Value;

        public IEnumerable<Indexed<int>> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x)
            .Select((x, i) => new Indexed<int>(i, x));

        [Test]
        public void Tests1()
        {
            var result = new List<Range>();

            Indexed<int> first = items.FirstOrDefault();
            Indexed<int> second = items.Skip(1).FirstOrDefault();
            int min = first.Index;
            int max = second.Index;
            Direction currentDirection = min <= max ? Direction.Ascending : Direction.Descending;

            foreach (var item in items.Skip(2))
            {
                first = second;
                second = item;
                max = second.Index;
                if (isDirectionChanged(first, second, currentDirection))
                {
                    result.Add(min..max);
                    currentDirection = first.Value < second.Value 
                        ? Direction.Ascending 
                        : Direction.Descending;
                        
                    min = second.Index;
                }
            }

            if (min != max)
                result.Add(min..max);

            var tmp = result.ToArray();
        }

        [Test]
        public void Tests2()
        {
            var tmp = items.Split(isMonotone, SplitOptions.IncludeBorders)
                .Select(x => new Range(x.First().Index, x.Last().Index))
                .ToArray();
        }

        [Test]
        public void Test3()
        {
            var items = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };
            Relation<int> isEqual = (x, y) => x == y;

            var result = items.SkipWhile(x => x == 0)
                .Split(isEqual)
                .Select(x => x.First())
                .ToArray();
        }

        [Test]
        public void Test4()
        {
            var items = new int[] { 0, 0, 0, 0, 0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4 };

            var tmp = items.Do(x => Debug.WriteLine($"first iteration {x}"))
                .SkipWhile(x => x == 0)
                .Do(x => Debug.WriteLine($"second iteration {x}"));
            
            var tmp2 = tmp.Skip(2).ToArray();
            var tmp3 = tmp.Take(2).ToArray();
        }
    }
}