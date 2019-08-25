using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class IteratorBench
    {
        public IEnumerable<int> items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };

        public Relation<int> relation = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

        [Benchmark]
        public int[] EnumerableIterator()
        {
            static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Relation<TSource> relation)
            {
                TSource prev = source.FirstOrDefault();

                if (source.Any())
                {
                    yield return prev;
                }

                foreach (TSource item in source.Skip(1))
                {
                    if (!relation(prev, item))
                        break;

                    prev = item;
                    yield return item;
                }
            }

            return TakeWhileIterator(items, relation).ToArray();
        }

        [Benchmark]
        public int[] EnumeratorIterator()
        {
            static IEnumerable<TSource> TakeWhileIterator<TSource>(IEnumerable<TSource> source, Relation<TSource> relation)
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    TSource prev = enumerator.Current;
                    yield return prev;

                    while (enumerator.MoveNext())
                    {
                        if (!relation(prev, enumerator.Current))
                            break;

                        prev = enumerator.Current;
                        yield return enumerator.Current;
                    }
                }
            }

            return TakeWhileIterator(items, relation).ToArray();
        }
    }
}