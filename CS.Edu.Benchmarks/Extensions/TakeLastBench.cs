using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class TakeLastBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 1000);

        [Benchmark]
        public int[] TakeLastArray()
        {
            return items.TakeLastArray(55).ToArray();
        }

        [Benchmark]
        public int[] TakeLastList()
        {
            return items.TakeLastList(55).ToArray();
        }

        [Benchmark]
        public int[] TakeLastLinkedList()
        {
            return items.TakeLastLinkedList(55).ToArray();
        }

        [Benchmark]
        public int[] TakeLastReverse()
        {
            return items.TakeLastReverse(55).ToArray();
        }

        [Benchmark]
        public int[] TakeLastSpan()
        {
            return items.TakeLastSpan(55).ToArray();
        }

        [Benchmark]
        public int[] TakeLastCore()
        {
            return items.TakeLast(55).ToArray();
        }
    }
}
