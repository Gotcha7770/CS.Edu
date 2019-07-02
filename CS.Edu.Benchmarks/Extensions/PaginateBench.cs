using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class PaginateBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 1000);
        public List<int> list = Enumerable.Range(0, 1000).ToList();

        [Benchmark]
        public void Paginate()
        {
            var tmp = items.Paginate(25).ToArray();
        }

        [Benchmark]
        public void Split()
        {
            var tmp = list.Split(25).ToArray();
        }
    }
}
