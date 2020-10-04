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

        [ParamsSource(nameof(ValuesForSource))]
        public (int Sought, IEnumerable<MyClass> Items) Source;

        public IEnumerable<(int, MyClass[])> ValuesForSource => new[] {100, 1000, 10000, 100000}
            .Select(x => (x, Enumerable.Range(0, x * 5)
                .Where(y => y.IsEven())
                .Select(y => new MyClass(y))
                .ToArray()));

        [Benchmark]
        public MyClass SimpleSearch()
        {
            return Source.Items.FirstOrDefault(x => !x.Value.IsEven())
                   ?? Source.Items.FirstOrDefault(x => x.Value == Source.Sought);
        }

        [Benchmark]
        public MyClass OptionalSearch()
        {
            return Source.Items.FirstOrOptional(x => !x.Value.IsEven())
                .ValueOr(() => Source.Items.FirstOrDefault(x => x.Value == Source.Sought));
        }

        [Benchmark]
        public MyClass SerialSearch()
        {
            return Source.Items.Find(x => !x.Value.IsEven())
                .ThenFind(x => x.Value == Source.Sought)
                .Result.Value;
        }

        [Benchmark]
        public MyClass ParallelSearch()
        {
            return Source.Items.Find(x => !x.Value.IsEven(), true)
                .ThenFind(x => x.Value == Source.Sought)
                .Result.Value;
        }

        [Benchmark]
        public MyClass MultiThreadSearch()
        {
            return FindMethod(Source.Items, x => !x.Value.IsEven(), x => x.Value == Source.Sought).Value;
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