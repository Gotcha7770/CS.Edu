using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks
{
    [Config(typeof(DefaultConfig))]
    public class MatrixBench
    {
        private readonly double[][] _mtrx = Enumerable.Range(0, 4)
            .Select(c => Enumerable.Range(0, 1000).Select(r => r * Math.Pow(10, c)).ToArray())
            .ToArray();

        [Benchmark]
        public double[] Where()
        {
            return _mtrx[2].Where((x, i) => { var value = _mtrx[0][i]; return value > 25 && value < 75; })
                .ToArray();            
        }

        [Benchmark]
        public double[] Select()
        {
            return _mtrx[0].Select((x, i) => ValueTuple.Create(x, i))
                .Where(x => x.Item1 > 25 && x.Item1 < 75)
                .Select(x => _mtrx[2][x.Item2])
                .ToArray();
        }

        [Benchmark]
        public double[] Cycle()
        {
            List<double> result = new List<double>();

            for (int i = 0; i < _mtrx[0].Length; i++)
            {
                var value = _mtrx[0][i];
                if (value > 25 && value < 75)
                {
                    result.Add(_mtrx[2][i]);
                }
            }

            return result.ToArray();
        }
    }
}
