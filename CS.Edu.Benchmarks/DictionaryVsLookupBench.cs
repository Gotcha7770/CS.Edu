using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks
{

    [Config(typeof(DefaultConfig))]
    public class DictionaryVsLookupBench
    {
        static Func<int, IEnumerable<(int, DateTime)>> f = x => new[]
            {
                // (x, new DateTime(1800 + x, 1, 1)),
                // (x, new DateTime(1900 + x, 1, 1)),
                // dictionary throws same key exception
                (x, new DateTime(2000 + x, 1, 1))
            };

        List<(int, DateTime)> _source = Enumerable.Range(0, 100)
            .SelectMany(x => f(x))
            .OrderBy(x => x.Item2.Year)
            .ToList();

        [Benchmark]
        public void ToDictionary()
        {
            var dictionary = _source.ToDictionary(x => x.Item1);
        }

        [Benchmark]
        public void ToLookup()
        {
            var dictionary = _source.ToLookup(x => x.Item1);
        }
    }
}