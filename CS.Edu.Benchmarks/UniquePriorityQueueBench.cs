using System;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Collections;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
public class UniquePriorityQueueBench
{
    private int[] _data = null!;

    [Params(1_000, 10_000, 100_000)] public int N;

    [GlobalSetup]
    public void Setup()
    {
        var rnd = new Random(42);
        _data = new int[N];
        for (int i = 0; i < N; i++)
            _data[i] = rnd.Next(0, N / 10); // добавляем дубликаты
    }

    [Benchmark]
    public int UniquePriorityQueue()
    {
        var pq = new UniquePriorityQueue<int>();
        foreach (var x in _data)
            pq.Enqueue(x);

        int sum = 0;
        while (pq.TryDequeue(out int value))
        {
            sum += value;
        }

        return sum;
    }

    [Benchmark]
    public int CollapsingPriorityQueue()
    {
        var pq = new CollapsingPriorityQueue<int>();
        foreach (var x in _data)
            pq.Enqueue(x);

        int sum = 0;
        while (pq.TryDequeue(out int value))
        {
            sum += value;
        }

        return sum;
    }
}