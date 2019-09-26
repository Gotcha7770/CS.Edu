using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class PaginateBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 1000);

        private readonly Consumer _consumer = new Consumer();

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
        public void PaginateConsume()
        {
            items.Paginate(25).Consume(_consumer);
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

        [Benchmark]
        public void BufferConsume()
        {
            items.Buffer(25).Consume(_consumer);
        }
    }
}