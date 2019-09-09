using NUnit.Framework;
using System;
using System.Linq;
using CS.Edu.Core.Extensions;
using CS.Edu.Core;
using System.Collections.Generic;

namespace CS.Edu.Tests.Temptests
{
    public enum Direction
    {
        Ascending,
        Descending
    }

    public class Indexed
    {
        public Indexed(int index, int value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; }

        public int Value { get; }
    }

    public class Range
    {
        public Range(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }

        public int End { get; }
    }

    [TestFixture]
    public class Tests
    {
        Relation<Indexed, Indexed, Indexed> isMonotone = (x, y, z) => x.Value < y.Value ? y.Value < z.Value : y.Value > z.Value;

        Relation<Indexed, Indexed, Direction> isDirectionChanged = (x, y, dir) => dir == Direction.Ascending ? x.Value > y.Value : x.Value < y.Value;

        public IEnumerable<Indexed> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x)
            .Select((x, i) => new Indexed(i, x));
            
        [Test]
        public void Tests1()
        {
            var result = new List<Range>();

            Indexed first = items.FirstOrDefault();
            Indexed second = items.Skip(1).FirstOrDefault();
            int min = first?.Index ?? 0;
            int max = second?.Index ?? 0;
            Direction currentDirection = min <= max ? Direction.Ascending : Direction.Descending;

            foreach (var item in items.Skip(2))
            {
                first = second;
                second = item;
                max = second.Index;
                if (isDirectionChanged(first, second, currentDirection))
                {
                    result.Add(new Range(min, max));
                    currentDirection = first.Value < second.Value ? Direction.Ascending : Direction.Descending;
                    min = second.Index;
                }
            }

            if (min != max)
                result.Add(new Range(min, max));

            var tmp = result.ToArray();
        }

        [Test]
        public void Tests2()
        {
            var tmp = items.Split(isMonotone)
                .Select(x => new Range(x.First().Index, x.Last().Index))
                .ToArray();
        }
    }
}