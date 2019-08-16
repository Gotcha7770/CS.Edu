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
        public IEnumerable<int>[] Paginate()
        {
            return items.Paginate(25).ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] PaginateToArray()
        {
            return items.Paginate(25).Select(x => x.ToArray()).ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] Split()
        {
            return list.Split(25).ToArray();
        }
    }
}
