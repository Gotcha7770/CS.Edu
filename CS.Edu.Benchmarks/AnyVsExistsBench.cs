using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class AnyVsExistsBench
{
    private readonly List<int> _items = Enumerable.Range(0, 1_000_000)
        .ToList();

    [Benchmark]
    public bool Any()
    {
        return _items.Any(x => x == 999999);
    }

    [Benchmark]
    public bool Exists()
    {
        return _items.Exists(x => x == 999999);
    }
}