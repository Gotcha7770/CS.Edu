using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class AggregateManyBench
{
    private IEnumerable<int> _items;

    static (int min, int max) Interactive(IEnumerable<int> source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Source is empty");

            int min = int.MaxValue, max = int.MinValue;

            do
            {
                min = Math.Min(enumerator.Current, min);
                max = Math.Max(enumerator.Current, max);

            } while (enumerator.MoveNext());

            return (min, max);
        }
    }

    static (int min, int max) Linq_Aggregate(IEnumerable<int> source)
    {
        return source.Aggregate((min: int.MaxValue, max: int.MinValue), (acc, curr) => (Math.Min(curr, acc.min), Math.Max(curr, acc.max)));
    }

    [GlobalSetup]
    public void GlobalSetup() => _items = EnumerableExtensions.Random(..5000, 10000);

    [Benchmark]
    public (int min, int max) GetMinMaxBench() => Interactive(_items);

    [Benchmark]
    public (int min, int max) AggregateBench() => Linq_Aggregate(_items);
}