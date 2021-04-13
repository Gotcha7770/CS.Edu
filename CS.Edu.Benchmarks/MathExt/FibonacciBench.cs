using BenchmarkDotNet.Attributes;
using CS.Edu.Core.MathExt;
using System.Linq;
using System.Reactive.Linq;

namespace CS.Edu.Benchmarks.Extensions
{
    [Config(typeof(DefaultConfig))]
    public class FibonacciBench
    {
        [Benchmark]
        public int FibonacciRecursiveBench()
        {
            return Fibonacci.Recursive(35);
        }

        [Benchmark]
        public int FibonacciIteratorBench()
        {
            return Fibonacci.Iterator().ElementAt(34);
        }

        [Benchmark]
        public int FibonacciLINQBench()
        {
            return Enumerable.Range(0, int.MaxValue)
                .Scan((X: 0, Y: 1), (acc, curr) => (acc.Y, acc.X + acc.Y))
                .ElementAt(34).X;
        }

        [Benchmark]
        public int FibonacciRxBench()
        {
            return Fibonacci.Observable(34)
                .First();
        }
    }
}