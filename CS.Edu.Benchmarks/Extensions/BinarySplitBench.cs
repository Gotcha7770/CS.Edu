using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
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

        internal int[] AppendExeptNull(int[] array, int value)
        {
            return (array, value) switch
            {
                (null, _) => new int[] { value },
                (int[] acc, 0) when acc.Last() == 0 => array,
                _ => array.Append(value).ToArray()
            };
        }

        internal List<int> AppendExeptNull2(List<int> list, int value)
        {
            if(list == null)
                return new List<int> { value };
             if(list.Last() != 0 || value != 0)
                list.Add(value);
            
            return list;
        }

        Func<int, bool> nonZero = x => x != 0;

        Relation<int> relation = new Relation<int>((x, y) => (x, y) switch
        {
            (0, 0) => true,
            (0, _) => false,
            (_, 0) => false,
            _ => true
        });

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
        public int[] SplitWithAggregate()
        {
            return items.Aggregate<int, int[]>(null, (acc, cur) => AppendExeptNull(acc, cur));
        }

        [Benchmark]
        public List<int> SplitWithAggregate2()
        {
            return items.Aggregate<int, List<int>>(null, (acc, cur) => AppendExeptNull2(acc, cur));
        }

        [Benchmark]
        public int[] SplitWithLINQ()
        {
            static IEnumerable<TSource> SpecialIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> constraint)
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

            return SpecialIterator(items, nonZero).ToArray();
        }

        [Benchmark]
        public int[] Split()
        {
            return items.Split(relation)
                .Select(x => x.All(y => y == 0) ? Enumerable.Repeat(x.First(), 1) : x)
                .SelectMany(x => x)
                .ToArray();
        }
    }
}
