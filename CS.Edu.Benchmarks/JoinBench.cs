using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class JoinBench
    {
        private readonly IEnumerable<int> Source = Enumerable.Range(0, 50);
        private readonly Consumer _consumer = new Consumer();

        [Benchmark]
        public void WhereQueryBench()
        {
            var query = from x in Source
                        from y in Source
                        where x * x == y
                        select (x, y);

            query.Consume(_consumer);
        }

        [Benchmark]
        public void JoinQueryBench()
        {
            var query = from x in Source
                        join y in Source on x * x equals y
                        select (x, y);

            query.Consume(_consumer);
        }
    }
}