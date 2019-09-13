using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class BinarySplitBench
    {
        // Задача свернуть последовательность { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 }
        // до такой {{1, 3}, {0, 0, 0}, {7}, {0,0}, {9}, {0}, {1}} -> {1, 3, 0, 7, 0, 9, 0, 1},
        // то есть редуцировать все группы нулей до одного

        public IEnumerable<int> items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };

        Relation<int> bothAreNotZero = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

        [Benchmark]
        public int[] SplitWithCycle()
        {
            List<int> result = new List<int>();

            bool isZero = false;
            foreach (var item in items)
            {
                if (item != 0)
                {
                    isZero = false;
                    result.Add(item);
                }
                else if (!isZero)
                {
                    isZero = true;
                    result.Add(item);
                }
            }

            return result.ToArray();
        }

        [Benchmark]
        public int[] Split()
        {
            return items.Split(bothAreNotZero)
                .Select(x => x.Any(y => y == 0) ? Enumerable.Repeat(x.First(), 1) : x)
                .SelectMany(x => x)
                .ToArray();
        }

        internal IEnumerable<int> Reduce(IEnumerable<int> source)
        {
            int first = source.First();
            if (first == 0)
                yield return first;
            else
            {
                foreach (var item in source)
                    yield return item;
            }
        }

        [Benchmark]
        public int[] SplitWithReduce()
        {
            return items.Split(bothAreNotZero)
                .Select(x => Reduce(x))
                .SelectMany(x => x)
                .ToArray();
        }

        [Benchmark]
        public int[] SplitWithReturn()
        {
            return items.Split(bothAreNotZero)
                .Select(x => x.First() == 0 ? EnumerableEx.Return(0) : x)
                .SelectMany(x => x)
                .ToArray();
        }

        // [Benchmark]
        // public int[] SplitUsingInteractiveExt()
        // {
        //     return items.Split(bothAreNotZero).If()
        // }
    }
}
