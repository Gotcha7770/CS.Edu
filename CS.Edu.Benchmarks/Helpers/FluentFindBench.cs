using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Helpers;
using DynamicData.Kernel;

namespace CS.Edu.Benchmarks.Helpers
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [Config(typeof(DefaultConfig))]
    public class FluentFindBench
    {
        public class MyClass
        {
            public int Value { get; }

            public MyClass(int value)
            {
                Value = value;
            }
        }

        private IEnumerable<MyClass> _items = Enumerable.Range(0, 20000)
            .Where(x => x.IsEven())
            .Select(x => new MyClass(x));

        [Benchmark]
        public MyClass SimpleSearch()
        {
            return _items.FirstOrDefault(x => !x.Value.IsEven())
                   ?? _items.FirstOrDefault(x => x.Value == 10000);
        }

        [Benchmark]
        public MyClass OptionalSearch()
        {
            return _items.FirstOrOptional(x => !x.Value.IsEven())
                .ValueOr(() => _items.FirstOrDefault(x => x.Value == 10000));
        }

        [Benchmark]
        public MyClass SerialSearch()
        {
            return _items.Find(x => !x.Value.IsEven())
                .ThenFind(x => x.Value == 10000)
                .Result.Value;
        }

        [Benchmark]
        public MyClass ParallelSearch()
        {
            return _items.Find(x => !x.Value.IsEven(), true)
                .ThenFind(x => x.Value == 10000)
                .Result.Value;
        }

        [Benchmark]
        public MyClass MultiThreadSearch()
        {
            return FindMethod(_items, x => !x.Value.IsEven(), x => x.Value == 10000).Value;
        }

        private Optional<T> FindMethod<T>(IEnumerable<T> items, params Predicate<T>[] predicates)
        {
            var results = new Optional<T>[predicates.Length];
            Parallel.ForEach(predicates, (cur, state, index) =>
            {
                using (var enumerator = items.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (cur(enumerator.Current))
                        {
                            results[index] = enumerator.Current;

                            if (index == 0)
                                state.Stop();

                            break;
                        }

                        if (state.IsStopped)
                            break;
                    }
                }
            });

            return results.FirstOrDefault(x => x.HasValue);
        }
    }
}