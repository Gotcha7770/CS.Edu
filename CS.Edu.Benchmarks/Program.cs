using System;
using BenchmarkDotNet.Running;
using CS.Edu.Benchmarks.Extensions;
using CS.Edu.Benchmarks.Helpers;
using CS.Edu.Benchmarks.Iterators;

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
                typeof(ListBench),
                typeof(FactorialBench),
                typeof(FibonacciBench),
                typeof(ToArrayVsToListBench),
                typeof(AggregateManyBench),
                typeof(PowerOfTwoBench),
                typeof(RangeDiffBench),
                typeof(InvalidateCollectionBench),
                typeof(JaggedVsRectArrayBench),
                typeof(DictionaryVsLookupBench),
                typeof(DoubleEnumerationBench),
                typeof(TestBench),
                typeof(PullOutBench),
                typeof(ExceptBench),
                typeof(TrimEnumerableStartBench),
                typeof(FluentFindBench)
            });

            switcher.Run();
            Console.ReadLine();
        }
    }
}
