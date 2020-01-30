using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class JaggedVsRectArrayBench
    {
        int[][] _jagged = Enumerable.Range(0, 50)
                .Select(c => Enumerable.Range(0, 50).Select(r => 1).ToArray())
                .ToArray();
                
        int[,] _2d = new int[50,50];

        [Benchmark]
        public int JaggedArrayAccess()
        {
            return _jagged[25][25];
        }

        [Benchmark]
        public int _2DArrayAccess()
        {
            return _2d[25,25];
        }
    }
}