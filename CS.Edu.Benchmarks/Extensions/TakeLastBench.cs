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
        public void TakeLastArray()
        {
            var tmp = items.TakeLastArray(55).ToArray();
        }

        [Benchmark]
        public void TakeLastList()
        {
            var tmp = items.TakeLastList(55).ToArray();
        }

        [Benchmark]
        public void TakeLastLinkedList()
        {
            var tmp = items.TakeLastLinkedList(55).ToArray();
        }

        [Benchmark]
        public void TakeLastReverse()
        {
            var tmp = items.TakeLastReverse(55).ToArray();
        }

        [Benchmark]
        public void TakeLastSpan()
        {
            var tmp = items.TakeLastSpan(55).ToArray();
        }

        [Benchmark]
        public void TakeLastCore()
        {
            var tmp = items.TakeLast(55).ToArray();
        }
    }
}
