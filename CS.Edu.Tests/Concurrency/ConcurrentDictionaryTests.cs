using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CS.Edu.Tests.Utils;
using NUnit.Framework;

namespace CS.Edu.Tests.Concurrency;

[TestFixture]
public class ConcurrentDictionaryTests
{
    //simultaneously

    private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

    [Test]
    public void TryRemoveTest()
    {
        List<bool> results = new List<bool>();
        var dic = new ConcurrentDictionary<Guid, Valuable<string>>();
        var queue = new ConcurrentQueue<Valuable<string>>();
        var cts = new CancellationTokenSource();

        bool Remove(Guid x)
        {
            Debug.WriteLine($"Try Remove: {x}");
            if (!dic.TryRemove(x, out _))
            {
                Debug.WriteLine("Cant remove!");
                return false;
            }

            return true;
        }

        var producer = Task.Run(() =>
        {
            for (int i = 0; i < 1_000_000; i++)
            {
                var item = Valuable.From($"Test{i}");
                if (dic.TryAdd(item.Key, item))
                {
                    queue.Enqueue(item);
                }

                Thread.Sleep(Random.Next(1, 3));
            }
        });

        var consumer = Task.Run(() =>
        {
            while (!cts.IsCancellationRequested)
            {
                Thread.Sleep(Random.Next(1, 3));

                if (queue.TryDequeue(out var item))
                {
                    var result = Remove(item.Key);
                    results.Add(result);
                }
            }
        });

        producer.Wait();
        cts.Cancel();

        EnumerableAssert.All(results, x => x);
    }

    [Test]
    public void METHOD()
    {
        List<bool> results = new List<bool>();
        var source = Enumerable.Range(0, 2_000)
            .Select(x => new Valuable<string, int>(x, $"Test{x}"))
            .Select(x => new KeyValuePair<int,Valuable<string, int>>(x.Key, x));
        var dic = new ConcurrentDictionary<int, Valuable<string, int>>(source);

        void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {message}");
        }

        void Add(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Log($"Try add {i}");
                if (!dic.TryAdd(i, new Valuable<string, int>(i, $"Test{i}")))
                {
                    Log($"Fail to add {i}");
                }
            }
        }

        void Remove(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                Log($"Try remove {i}");
                if (dic.TryRemove(i, out _))
                {
                    results.Add(true);
                }
                else
                {
                    Log($"Fail to remove {i}");
                    results.Add(false);
                }
            }
        }

        Task.WaitAll(Task.Run(() => Add(2_000, 3_000)),
            Task.Run(() => Remove(0, 1_000)),
            Task.Run(() => Remove(1_000, 2_000)));

        // Parallel.Invoke
        // (
        //     () => Add(2_000, 3_000),
        //     () => Remove(0, 1_000),
        //     () => Remove(1_000, 2_000)
        // );

        EnumerableAssert.All(results, x => x);
    }
}