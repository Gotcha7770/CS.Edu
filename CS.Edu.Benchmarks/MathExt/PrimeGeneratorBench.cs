using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.MathExt;
using System.Linq;

namespace CS.Edu.Benchmarks.Extensions
{
    [Config(typeof(DefaultConfig))]
    public class PrimeGeneratorBench
    {
        private readonly Consumer _consumer = new Consumer();
        
        [Benchmark]
        public void ConsumePrimesGenerator()
        {
            PrimesGenerator.GetPrimes()
                .Take(1000)
                .Consume(_consumer);
        }

        [Benchmark]
        public void ConsumeCustomPrimesQuery()
        {
            EnumerableEx.Generate(2L, x => x < long.MaxValue, x => ++x, x => x)
                .Where(x => x.IsPrime())
                .Take(1000)
                .Consume(_consumer);
        }
    }
}