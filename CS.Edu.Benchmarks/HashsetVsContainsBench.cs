using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using MathNet.Numerics.Random;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class HashsetVsContainsBench
{
    private readonly Consumer _consumer = new Consumer();
    private readonly Random _random = new Random((int)DateTime.Now.Ticks);

    public record Item(byte Id);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void ArrayContainsBench(byte[] indexes, Item[] items)
    {
        items.Where(x => indexes.Contains(x.Id)).Consume(_consumer);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void HashSetContainsBench(byte[] indexes, Item[] items)
    {
        var set = indexes.ToHashSet();
        items.Where(x => set.Contains(x.Id)).Consume(_consumer);
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public void SearchValuesContainsBench(byte[] indexes, Item[] items)
    {
        var values = SearchValues.Create(indexes);
        items.Where(x => values.Contains(x.Id)).Consume(_consumer);
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
            GetNumbers(1_000),
            GetItems(1_000)
        };
        yield return new object[]
        {
            GetNumbers(10_000),
            GetItems(10_000)
        };
    }

    private byte[] GetNumbers(int count)
    {
        return _random.NextBytes(count);
    }

    private Item[] GetItems(int count)
    {
        return _random.NextBytes(count)
            .Select(x => new Item(x))
            .ToArray();
    }
}