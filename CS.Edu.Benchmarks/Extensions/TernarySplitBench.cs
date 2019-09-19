using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using Range = System.Range;

namespace CS.Edu.Benchmarks.Extensions
{
    public enum Direction
    {
        Ascending,
        Descending
    }

    [Config(typeof(DefaultConfig))]
    public class TernarySplitBench
    {
        //Задача разделить последовательность чисел на подпоследовательности,
        //в которых числа частично упорядочены, 
        //вернуть интервал - первый и последний индекс каждой последовательности
        //границы интервалов включены -> [0;5] [5;10] и т. д.

        public IEnumerable<Indexed<int>> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x)
            .Select((x, i) => new Indexed<int>(i, x));

        Relation<Indexed<int>, Indexed<int>, Indexed<int>> isMonotone = (x, y, z) => x.Value <= y.Value ? y.Value <= z.Value : y.Value > z.Value;

        Relation<Indexed<int>, Indexed<int>, Direction> isDirectionChanged = (x, y, dir) => dir == Direction.Ascending ? x.Value > y.Value : x.Value < y.Value;

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
                    currentDirection = first.Value < second.Value ? Direction.Ascending : Direction.Descending;
                    min = second.Index;
                }
            }

            if (min != max)
                result.Add(min..max);

            return result.ToArray();
        }
    }
}
