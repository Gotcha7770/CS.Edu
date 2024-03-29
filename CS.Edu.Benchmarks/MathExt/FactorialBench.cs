using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.MathExt;

namespace CS.Edu.Benchmarks.MathExt
{
    [Config(typeof(DefaultConfig))]
    public class FactorialBench
    {
        [Benchmark]
        public int FactorialRecursiveBench()
        {
            return Factorial.Recursive(12);
        }

        [Benchmark]
        public int FactorialIteratorBench()
        {
            return Factorial.Iterator().ElementAt(12);
        }

        [Benchmark]
        public int FactorialLINQBench()
        {
            return Enumerable.Range(0, 13)
                .Aggregate((acc, cur) => acc == 0 ? 1 : acc * cur);
        }
    }
}