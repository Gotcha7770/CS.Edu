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

    [Config(typeof(DefaultConfig))]
    public class TernarySplitBench
    {
        //Задача разделить последовательность чисел на подпоследовательности,
        //в которых числа строго возрастают или строго убывают

        public IEnumerable<int> items = Enumerable.Range(0, 1000)
            .Paginate(50)
            .Select((x, i) => i.IsEven() ? x : x.Reverse())
            .SelectMany(x => x);

        Relation<int, int, int> isMonotone = (x, y, z) => x < y ? y < z : y > z;

        [Benchmark]
        public IEnumerable<int>[] Split()
        {
            return items.Split(isMonotone).ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] SplitToArray()
        {
            return items.Split(isMonotone).Select(x => x.ToArray()).ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] SplitWithCycle()
        {
            List<IEnumerable<int>> result = new List<IEnumerable<int>>();
            List<int> accumulate = new List<int>();

            Direction currentDirection = 0;
            foreach (var item in items)
            {
                if (accumulate.Count == 1)
                {
                    currentDirection = accumulate.First() < item ? Direction.Ascending : Direction.Descending;
                }
                else if (accumulate.Count > 1)
                {
                    if (currentDirection == Direction.Ascending ? accumulate.Last() > item : accumulate.Last() < item)
                    {
                        result.Add(accumulate);
                        accumulate = new List<int>();
                    }
                }

                accumulate.Add(item);
            }

            return result.ToArray();
        }
    }
}
