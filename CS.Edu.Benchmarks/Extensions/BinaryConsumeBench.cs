using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    public class ConsumerMock<T>
    {
        private readonly List<T> _storage = new List<T>();
        private int _changeCount;

        public void Append(T value)
        {
            _storage.Add(value);
            OnDataChanged();
            //Calculation
        }

        public void Append(IEnumerable<T> values)
        {
            _storage.AddRange(values);
            OnDataChanged();
            //Calculation
        }

        private void OnDataChanged()
        {
            IncrementChangeCount();
        }

        private void IncrementChangeCount()
        {
            Interlocked.Increment(ref _changeCount);
        }
    }

    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class BinaryConsumeBench
    {
        public ConsumerMock<int> Consumer { get; private set; }
        public IEnumerable<int> items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };

        Relation<int> bothAreZeroOrNot = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

        [IterationSetup]
        public void IterationSetup()
        {
            Consumer = new ConsumerMock<int>();
        }

        [Benchmark]
        public void FillWithCycle()
        {
            List<int> result = new List<int>();

            bool isZero = false;
            foreach (var item in items)
            {
                if (item != 0)
                {
                    isZero = false;
                    result.Add(item);
                }
                else if (!isZero)
                {
                    isZero = true;
                    result.Add(item);
                }
            }

            Consumer.Append(result);
        }

        static IEnumerable<IEnumerable<int>> PlainSplitIterator(IEnumerable<int> source, Relation<int> relation)
        {
            while (source.Any())
            {
                yield return source.TakeWhile(relation);
                source = source.SkipWhile(relation);
            }
        }

        [Benchmark]
        public void PlainSplit()
        {
            var result = PlainSplitIterator(items, bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            Consumer.Append(result);
        }

        [Benchmark]
        public void Split()
        {
            var result = items.Split(bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            Consumer.Append(result);
        }
    }
}