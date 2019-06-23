using BenchmarkDotNet.Running;
using CS.Edu.Benchmarks.Extensions;

namespace CS.Edu.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PaginateBench>();
        }
    }
}
