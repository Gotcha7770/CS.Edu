using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class ToArrayVsToListBench
    {
        IEnumerable<int> items = Enumerable.Range(0, 100000);

        [Benchmark]
        public int[] ToArrayBench()
        {
            return items.ToArray();
        }

        [Benchmark]
        public List<int> ToListBench()
        {
            return items.ToList();
        }
    }
}