using System;
using BenchmarkDotNet.Running;
using CS.Edu.Benchmarks.Extensions;

namespace CS.Edu.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[] {
                typeof(FirstOrDefaultTypeBench),
                typeof(RelationBench),
                typeof(IsEvenBench),
                typeof(IteratorBench),
                typeof(PaginateBench),
                typeof(BinarySplitBench),
                typeof(TernarySplitBench)
            });

            switcher.Run();
            Console.ReadLine();
        }
    }
}
