using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Benchmarks.Helpers;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class HashsetVsContainsBench
{
    private readonly Consumer _consumer = new Consumer();
    private readonly Random _random = new Random((int)DateTime.Now.Ticks);

    public record Item(int Id);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void ArrayContainsBench(int[] indexes, Item[] items)
    {
        items.Where(x => indexes.Contains(x.Id)).Consume(_consumer);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void HashSetContainsBench(int[] indexes, Item[] items)
    {
        var set = indexes.ToHashSet();
        items.Where(x => set.Contains(x.Id)).Consume(_consumer);
    }

    public IEnumerable<object[]> Data()
    {
        yield return new object[]
        {
            GetNumbers(10),
            GetItems(10)
        };
        yield return new object[]
        {
            GetNumbers(100),
            GetItems(100)
        };
        yield return new object[]
        {
            GetNumbers(1000),
            GetItems(1000)
        };
    }

    public int[] GetNumbers(int count)
    {
        var array = Enumerable.Range(0, count).ToArray();
        _random.Shuffle(array);

        return array;
    }

    public Item[] GetItems(int count)
    {
        var array = Enumerable.Range(0, count)
            .Where(x => x % 3 == 0)
            .Select(x => new Item(x))
            .ToArray();

        _random.Shuffle(array);

        return array;
    }
}