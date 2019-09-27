using System;
using BenchmarkDotNet.Running;
using CS.Edu.Benchmarks.Extensions;
using CS.Edu.Benchmarks.PropertySynchronization;

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
                typeof(TernarySplitBench),
                typeof(PropertySynchronizationBench),
                typeof(ListBench)
            });

            switcher.Run();
            Console.ReadLine();
        }
    }
}
