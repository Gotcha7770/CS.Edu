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

        Relation<int> bothAreZeroOrNot = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

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

        static IEnumerable<IEnumerable<int>> PlainSplitIterator(IEnumerable<int> source, Relation<int> relation)
        {
            while (source.Any())
            {
                yield return source.TakeWhile(relation);
                source = source.SkipWhile(relation);
            }
        }

        [Benchmark]
        public int[] PlainSplit()
        {
            return PlainSplitIterator(items, bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x)
                .ToArray();
        }

        [Benchmark]
        public int[] Split()
        {
            return items.Split(bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x)
                .ToArray();
        }        

        [Benchmark]
        public int[] IxSplit()
        {
            return Array.Empty<int>();
            //return items.Split(bothAreZeroOrNot).If()
        }

        [Benchmark]
        public int[] RxSplit()
        {
            return Array.Empty<int>();
        }
    }
}
