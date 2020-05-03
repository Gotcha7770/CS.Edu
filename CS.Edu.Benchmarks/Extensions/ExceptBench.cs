using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class ExceptBench
    {
        private readonly Consumer _consumer = new Consumer();
        private readonly IEnumerable<int> _source = Enumerable.Range(0, 1000)
            .Select(x => x % 4 == 0 && x % 6 == 0 ? 0 : x);

        [Benchmark]
        public void ExceptWithIEnumerableExcept()
        {
            _source.Except(EnumerableEx.Return(0)).Consume(_consumer);
        }

        [Benchmark]
        public void ExceptWithIEnumerableWhere()
        {
            _source.Where(x => x != 0).Consume(_consumer);
        }
    }
}