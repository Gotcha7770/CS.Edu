using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class DoubleEnumerationBench
    {
        private readonly IEnumerable<int> _items = Enumerable.Range(0, 1000000);

        private readonly Consumer _consumer = new Consumer();

        private IEnumerable<TOut> Iterator<TIn, TOut>(IEnumerable<TIn> source, Func<TIn, TIn, TOut> selector)
        {
            TIn[] array = source.ToArray();

            for(int i = 0, j = 1; j < array.Length; i++, j++)
            {
                yield return selector(array[i], array[j]);
            }
        }

        [Benchmark]
        public void ArrayWithIndexAccess()
        {
            var result = Iterator(_items, (x, y) => (x, y));

            _consumer.Consume(result);
        }

        [Benchmark]
        public void ZipWithSkipOne()
        {
            var result = _items.Zip(_items.Skip(1));

            _consumer.Consume(result);
        }

        [Benchmark]
        public (int, int) ArrayWithIndexAccessSecond()
        {
            return Iterator(_items, (x, y) => (x, y)).ElementAt(1);
        }

        [Benchmark]
        public (int, int) ZipWithSkipOneSecond()
        {
            return _items.Zip(_items.Skip(1)).ElementAt(1);
        }
    }
}