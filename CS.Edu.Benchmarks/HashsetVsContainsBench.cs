using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class HashsetVsContainsBench
{
    public class Item
    {
        public Item(int id) => Id = id;

        public int Id { get; }
        public bool IsValid { get; set; }
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public Item[] ContainsBench(int[] indexes, Item[] items)
    {
        foreach (Item item in items)
        {
            if (indexes.Contains(item.Id))
            {
                item.IsValid = true;
            }
        }

        return items;
    }

    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public Item[] HashSetBench(int[] indexes, Item[] items)
    {
        foreach (Item item in items.IntersectBy(indexes, x => x.Id))
        {
            item.IsValid = true;
        }

        return items;
    }

    public IEnumerable<object[]> Data()
    {
        yield return new object[]
        {
            Enumerable.Range(0, 10).ToArray(),
            Enumerable.Range(0, 10).Where(x => x % 3 == 0).Select(x => new Item(x)).ToArray()
        };
        yield return new object[]
        {
            Enumerable.Range(0, 100).ToArray(),
            Enumerable.Range(0, 100).Where(x => x % 3 == 0).Select(x => new Item(x)).ToArray()
        };
        yield return new object[]
        {
            Enumerable.Range(0, 1000).ToArray(),
            Enumerable.Range(0, 1000).Where(x => x % 3 == 0).Select(x => new Item(x)).ToArray()
        };
        yield return new object[]
        {
            Enumerable.Range(0, 10000).ToArray(),
            Enumerable.Range(0, 10000).Where(x => x % 3 == 0).Select(x => new Item(x)).ToArray()
        };
    }
}