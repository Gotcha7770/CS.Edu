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
        //границы интервалов включены -> [0;5] [10;5] и т. д.

        public IEnumerable<int> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x);            

        Relation<int, int, int> isMonotone = (x, y, z) => x <= y ? y <= z : y > z;

        Relation<(int, int), (int, int), Direction> isDirectionChanged = (x, y, dir) => dir == Direction.Ascending ? x.Item2 > y.Item2 : x.Item2 < y.Item2;

        [Benchmark]
        public Range[] Split()
        {
            return items.Split(isMonotone, SplitOptions.IncludeBorders)
                .Select(x => new Range(x.First(), x.Last()))
                .ToArray();
        }

        [Benchmark]
        public Range[] SplitWithCycle()
        {
            // var result = new List<Range>();

            // (int, int) first = items.FirstOrDefault();
            // (int, int) second = items.Skip(1).FirstOrDefault();
            // int min = first.Item1;
            // int max = second.Item1;
            // Direction currentDirection = min <= max ? Direction.Ascending : Direction.Descending;

            // foreach (var item in items.Skip(2))
            // {
            //     first = second;
            //     second = item;
            //     max = second.Item1;
            //     if (isDirectionChanged(first, second, currentDirection))
            //     {
            //         result.Add(min..max);
            //         currentDirection = first.Item2 < second.Item2 ? Direction.Ascending : Direction.Descending;
            //         min = second.Item1;
            //     }
            // }

            // if (min != max)
            //     result.Add(min..max);

            // return result.ToArray();

            return Array.Empty<Range>();
        }
    }
}
