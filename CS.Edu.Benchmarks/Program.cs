using System;
using BenchmarkDotNet.Running;
using CS.Edu.Benchmarks;
using CS.Edu.Benchmarks.Extensions;

var switcher = new BenchmarkSwitcher(new[]
{
    typeof(FirstOrDefaultTypeBench),
    typeof(RelationBench),
    typeof(IsEvenBench),
    typeof(IteratorBench),
    typeof(PaginateBench),
    typeof(BinarySplitBench),
    typeof(TernarySplitBench),
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
    typeof(PullOutBench),
    typeof(ExceptBench),
    typeof(JoinBench),
    typeof(AggregateBench),
    typeof(EnumerableCreateBench),
    typeof(MemberAccessBench),
    typeof(EnumerableVsArrayCreation)
});

switcher.Run();
Console.ReadLine();