using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
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

    [Config(typeof(DefaultConfig))]
    public class TernarySplitBench
    {
        //Задача разделить последовательность чисел на подпоследовательности,
        //в которых числа частично упорядочены, 
        //вернуть интервал - первый и последний индекс каждой последовательности
        //границы интервалов включены -> [0;5] [5;10] и т. д.

        public IEnumerable<Indexed> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x)
            .Select((x, i) => new Indexed(i, x));

        Relation<Indexed, Indexed, Indexed> isMonotone = (x, y, z) => x.Value < y.Value ? y.Value < z.Value : y.Value > z.Value;

        Relation<Indexed, Indexed, Direction> isDirectionChanged = (x, y, dir) => dir == Direction.Ascending ? x.Value > y.Value : x.Value < y.Value;

        [Benchmark]
        public Range[] Split()
        {
            return items.Split(isMonotone, SplitOptions.IncludeBorders)
                .Select(x => new Range(x.First().Index, x.Last().Index))
                .ToArray();
        }

        [Benchmark]
        public Range[] SplitWithCycle()
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

            return result.ToArray();
        }
    }
}
