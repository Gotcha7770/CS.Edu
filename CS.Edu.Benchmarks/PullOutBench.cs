using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class PullOutBench
    {
        private readonly IEnumerable<int> _input = Enumerable.Range(0, 10000);
        private readonly Consumer _consumer = new Consumer();

        [Benchmark]
        public void UsingDictionary()
        {
            var dic = _input.ToDictionary(x => x, x => -x);

            dic.Keys.ToArray().Consume(_consumer);
            dic.Values.ToArray().Consume(_consumer);
        }

        [Benchmark]
        public void UsingPullOut()
        {
            PullOut(_input, x => x, x => -x, out int[] xValues, out int[] yValues);

            xValues.Consume(_consumer);
            yValues.Consume(_consumer);
        }

        void PullOut<TIn, TOut1, TOut2>(IEnumerable<TIn> source,
                                        Func<TIn, TOut1> selector1,
                                        Func<TIn, TOut2> selector2,
                                        out TOut1[] output1,
                                        out TOut2[] output2)
        {
            output1 = source.Select(selector1).ToArray();
            output2 = source.Select(selector2).ToArray();
        }
    }
}