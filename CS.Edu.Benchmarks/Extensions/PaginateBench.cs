using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [Config(typeof(DefaultConfig))]
    public class PaginateBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 100);
        
        [Benchmark]
        public void PrettyPaginate()
        {
            items.Paginate(10).ToArray();
        }
    }
}
