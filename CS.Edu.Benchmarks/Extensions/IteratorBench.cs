using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Iterators;
using DynamicData.Kernel;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class IteratorBench
    {
        public IEnumerable<int> items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };
        public Relation<int> relation = (x, y) => x == 0 ? y == 0 : y != 0;
        private readonly Consumer _consumer = new Consumer();

        [Benchmark]
        public void EnumerableIterator()
        {
            static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source, Relation<T> relation)
            {
                T prev = source.FirstOrDefault();

                if (source.Any())
                {
                    yield return prev;
                }

                foreach (T item in source.Skip(1))
                {
                    if (!relation(prev, item))
                        break;

                    prev = item;
                    yield return item;
                }
            }

            TakeWhileIterator(items, relation).Consume(_consumer);
        }

        [Benchmark]
        public void EnumeratorIterator()
        {
            static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source, Relation<T> relation)
            {
                using (var enumerator = source.GetEnumerator())
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    T prev = enumerator.Current;
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

            TakeWhileIterator(items, relation).Consume(_consumer);
        }

        [Benchmark]
        public void PrevNextIterator()
        {
            static IEnumerable<T> TakeWhileIterator<T>(IEnumerable<T> source,
                Relation<T> relation)
            {
                foreach (PrevNextValue<T> item in source.ToPrevNextIterator())
                {
                    if (item.Next == Optional<T>.None || !relation(item.Current, (T)item.Next))
                    {
                        yield return item.Current;
                        break;
                    }

                    yield return item.Current;
                }
            }

            TakeWhileIterator(items, relation).Consume(_consumer);
        }
    }
}