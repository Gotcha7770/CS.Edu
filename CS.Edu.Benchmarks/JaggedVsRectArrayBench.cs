using System.Linq;
using BenchmarkDotNet.Attributes;
using MathNet.Numerics.LinearAlgebra;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class JaggedVsRectArrayBench
    {
        double[][] _jagged = Enumerable.Range(0, 50)
                .Select(c => Enumerable.Range(0, 50).Select(r => (double)1).ToArray())
                .ToArray();

        double[,] _2d = new double[50,50];

        Matrix<double> _matrix = Matrix<double>.Build.Dense(50, 50);

        [Benchmark]
        public double JaggedArrayAccess()
        {
            return _jagged[25][25];
        }

        [Benchmark]
        public double _2DArrayAccess()
        {
            return _2d[25,25];
        }

        [Benchmark]
        public double MatrixAccess()
        {
            return _matrix[25, 25];
        }
    }
}