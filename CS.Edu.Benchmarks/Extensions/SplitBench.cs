using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [Config(typeof(DefaultConfig))]
    public class SplitBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 1000).Paginate(50).Select((x, i) => i.IsEven() ? x : x.Reverse()).SelectMany(x => x);
        public Relation<int> lessThan = (x, y) => x < y;

        [Benchmark]
        public IEnumerable<int>[] Split()
        {
            return items.Split(lessThan).ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] SplitToArray()
        {
            return items.Split(lessThan).Select(x => x.ToArray()).ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] SplitWithCycle()
        {
            List<int> result = new List<int>();
            int prev = items.First();
            foreach (var item in items.Skip(1))
            {
                if(lessThan(prev, item))
            }

            return Enumerable.Empty<IEnumerable<int>>().ToArray();
        }
    }
}
