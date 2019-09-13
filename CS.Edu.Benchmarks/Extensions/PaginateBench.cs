using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class PaginateBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 1000);

        [Benchmark]
        public IEnumerable<int>[] Paginate()
        {
            return items.Paginate(25).ToArray();
        }

        [Benchmark]
        public int[][] PaginateToArray()
        {
            return items.Paginate(25).Select(x => x.ToArray()).ToArray();
        }

        [Benchmark]
        public int[][] LastPage()
        {
            return Enumerable.Repeat(items.Paginate(25).Last(), 25)
                .Select(x => x.ToArray())
                .ToArray();
        }

        [Benchmark]
        public IEnumerable<int>[] Buffer()
        {
            return items.Buffer(25).ToArray();
        }

        [Benchmark]
        public int[][] BufferToArray()
        {
            return items.Buffer(25).Select(x => x.ToArray()).ToArray();
        }

        [Benchmark]
        public int[][] LastBuffer()
        {
            return Enumerable.Repeat(items.Buffer(25).Last(), 25)
                .Select(x => x.ToArray())
                .ToArray();
        }
    }
}