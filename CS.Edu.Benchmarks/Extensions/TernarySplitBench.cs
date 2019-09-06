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
        //в которых числа строго возрастают или строго убывают, 
        //вернуть интервал - первый и последний индекс каждой последовательности

        public IEnumerable<Indexed> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x)
            .Select((x, i) => new Indexed(i, x));

        Relation<Indexed, Indexed, Indexed> isMonotone = (x, y, z) => x.Value < y.Value ? y.Value < z.Value : y.Value > z.Value;

        [Benchmark]
        public Range[] Split()
        {
            return items.Split(isMonotone)
                .Select(x => new Range(x.First().Index, x.Last().Index))
                .ToArray();
        }

        // static IEnumerable<T[]> SpecialIterator<T>(IEnumerable<T> source, Relation<T, T, T> relation)
        // {

        // }

        [Benchmark]
        public Range[] SplitWithCycle()
        {
            //return SpecialIterator(items, isMonotone).ToArray();
            return Array.Empty<Range>();
        }
    }
}
