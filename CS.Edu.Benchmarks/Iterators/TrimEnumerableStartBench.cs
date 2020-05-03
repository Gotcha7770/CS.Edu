﻿using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Iterators;

namespace CS.Edu.Benchmarks.Iterators
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class TrimEnumerableStartBench
    {
        private readonly IEnumerable<int> _input = Enumerable.Repeat(0, 1000)
            .Concat(Enumerable.Range(1, 2000))
            .Concat(Enumerable.Repeat(0, 1000))
            .Concat(Enumerable.Range(1, 2000))
            .Concat(Enumerable.Repeat(0, 1000));

        private readonly Consumer _consumer = new Consumer();

        [Benchmark]
        public void TrimStartWithLINQ()
        {
            _input.SkipWhile(x => x == 0).Consume(_consumer);
        }

        [Benchmark]
        public void TrimStartAndReplaceWithLINQ()
        {
            _input.SkipWhile(x => x == 0)
                .Select(x => x == 0 ? -1 : x)
                .Consume(_consumer);
        }

        [Benchmark]
        public void TrimStartWithTrimIterator()
        {
            TrimStartTestIterator(_input, 0).Consume(_consumer);
        }

        [Benchmark]
        public void TrimStartAndReplaceWithTrimIterator()
        {
            TrimStartAndReplaceTestIterator(_input, 0).Consume(_consumer);
        }

        private IEnumerable<int> TrimStartTestIterator(IEnumerable<int> input, int toTrim)
        {
            using (var iterator = new TrimStartIterator<int>(input, toTrim))
            {
                while (iterator.MoveNext())
                {
                    yield return iterator.Current;
                }
            }
        }

        private IEnumerable<int> TrimStartAndReplaceTestIterator(IEnumerable<int> input, int toTrim)
        {
            using (var iterator = new TrimStartIterator<int>(input, toTrim))
            {
                while (iterator.MoveNext())
                {
                    if (iterator.Current == toTrim)
                    {
                        yield return -1;
                    }

                    yield return iterator.Current;
                }
            }
        }
    }
}