using System;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.MathExt;

namespace CS.Edu.Benchmarks.MathExt
{
    [Config(typeof(DefaultConfig))]
    public class PowerOfTwoBench
    {
        [Benchmark]
        public long SystemMathPower()
        {
            return (long)Math.Pow(2, 32);
        }

        [Benchmark]
        public long PowerWithBitwiseOperator()
        {
            return Number.PowerOfTwo(32);
        }
    }
}