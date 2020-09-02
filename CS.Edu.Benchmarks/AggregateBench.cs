using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class AggregateBench
    {
        private readonly IEnumerable<IEnumerable<int>> _source = Enumerable.Range(0, 10).Select(x => ( x, "Group1"))
            .Concat(Enumerable.Range(0, 5).Select(x => (x, "Group2")))
            .Concat(Enumerable.Range(3, 4).Select(x => (x, "Group3")))
            .Concat(Enumerable.Range(5, 5).Select(x => (x, "Group4")))
            .Concat(Enumerable.Range(0, 10).Select(x => (x, "Group5")))
            .Concat(Enumerable.Range(0, 5).Select(x => (x, "Group6")))
            .Concat(Enumerable.Range(3, 4).Select(x => (x, "Group7")))
            .Concat(Enumerable.Range(5, 5).Select(x => (x, "Group8")))
            .GroupBy(x => x.Item2)
            .Select(x => x.Select(y => y.Item1));

        private readonly Consumer _consumer = new Consumer();

        [Benchmark]
        public void Aggragate1()
        {
            var query = _source.Skip(1)
                .Aggregate(new HashSet<int>(_source.First()), (agg, cur) =>
                {
                    agg.IntersectWith(cur);
                    return agg;
                });

            query.Consume(_consumer);
        }

        [Benchmark]
        public void Aggragate2()
        {
            var query = _source.Aggregate((HashSet<int>)null, (agg, cur) =>
                {
                    if (agg == null)
                        agg = new HashSet<int>(cur);
                    else
                        agg.IntersectWith(cur);

                    return agg;
                });

            query.Consume(_consumer);
        }

        [Benchmark]
        public void Aggragate3()
        {
            var query = _source.Aggregate(Enumerable.Empty<int>(), (agg, cur) => agg.IsNullOrEmpty()
                ? cur
                : agg.Intersect(cur));

            query.Consume(_consumer);
        }
    }
}