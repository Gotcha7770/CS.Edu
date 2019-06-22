using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;
using System.Collections.Generic;

namespace CS.Edu.Benchmarks.Extensions
{
    [Config(typeof(DefaultConfig))]
    public class IsEvenBench
    {
        [ParamsSource(nameof(Numbers))]
        public int Number { get; set; }

        public IEnumerable<int> Numbers => new[] { 0, 1, 2, 3, 4, 5, 10, 11, 122, 123, 1234, 1235, 12344, 12345, 123456, 123457 };

        [Benchmark]
        public bool StandardCheck()
        {
            return Number % 2 == 0;
        }

        [Benchmark]
        public bool ExtCheck()
        {
            return Number.IsEven();
        }
    }
}
