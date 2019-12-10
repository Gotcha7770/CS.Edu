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
        public void ConsumeCustomPrimesQueryWithGenerate()
        {
            EnumerableEx.Generate(3L, x => x < long.MaxValue, x => x +=2, x => x)                
                .Where(x => x.IsPrime())                
                .Prepend(2)
                .Take(1000)
                .Consume(_consumer);
        }

        [Benchmark]
        public void ConsumeCustomPrimesQueryWithRange()
        {
            LongExt.Range(3, long.MaxValue - 2, 2)                
                .Where(x => x.IsPrime())                
                .Prepend(2)
                .Take(1000)
                .Consume(_consumer);
        }
    }
}