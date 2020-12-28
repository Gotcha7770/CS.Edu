using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Helpers;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class EnumerableCreateBench
    {
        private readonly Consumer _consumer = new Consumer();

        [Benchmark]
        public void ListAsEnumerable()
        {
            IEnumerable<DateTime> enumerable = new List<DateTime>
            {
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now
            };

            _consumer.Consume(enumerable);
        }

        [Benchmark]
        public void ArrayAsEnumerable()
        {
            IEnumerable<DateTime> enumerable = new []
            {
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now
            };

            _consumer.Consume(enumerable);
        }

        [Benchmark]
        public void EnumerableConcatenation()
        {
            IEnumerable<DateTime> enumerable = EnumerableEx.Concat(
                EnumerableEx.Return(DateTime.Now),
                EnumerableEx.Return(DateTime.Now),
                EnumerableEx.Return(DateTime.Now),
                EnumerableEx.Return(DateTime.Now),
                EnumerableEx.Return(DateTime.Now));

            _consumer.Consume(enumerable);
        }

        [Benchmark]
        public void EnumerableFromYielder()
        {
            IEnumerable<DateTime> enumerable = EnumerableEx.Create<DateTime>(async yielder =>
            {
                await yielder.Return(DateTime.Now);
                await yielder.Return(DateTime.Now);
                await yielder.Return(DateTime.Now);
                await yielder.Return(DateTime.Now);
                await yielder.Return(DateTime.Now);
            });

            _consumer.Consume(enumerable);
        }

        [Benchmark]
        public void EnumerableFromEnumerator()
        {
            IEnumerable<DateTime> enumerable = EnumerableEx.Create(() => Enumerator.Create(
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now));

            _consumer.Consume(enumerable);
        }

        [Benchmark]
        public void EnumerableFromLocalFunction()
        {
            IEnumerable<DateTime> enumerable = _(); IEnumerable<DateTime> _()
            {
                yield return DateTime.Now;
                yield return DateTime.Now;
                yield return DateTime.Now;
                yield return DateTime.Now;
                yield return DateTime.Now;
            }

            _consumer.Consume(enumerable);
        }
    }
}