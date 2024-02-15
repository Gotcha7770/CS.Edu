using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class InvalidateCollectionBench
    {
        private record Data(int Value, DateTime Date);

        private readonly List<Data> _source =
        [
            new Data(0, new DateTime(2012, 2, 1)),
            new Data(10, new DateTime(2013, 3, 1)),
            new Data(20, new DateTime(2014, 5, 1)),
            new Data(30, new DateTime(2015, 7, 1)),
            new Data(40, new DateTime(2016, 11, 1))
        ];

        private readonly Data[] _update =
        [
            new Data(0, new DateTime(2012, 2, 2)),
            new Data(12, new DateTime(2013, 3, 3)),
            new Data(22, new DateTime(2014, 4, 4)),
            new Data(30, new DateTime(2015, 5, 2))
        ];

        private Dictionary<int, Data> _dic;

        private readonly Func<Data, Data, Data> _mergeFunc = (x, y) => x with { Date = y.Date };
        private readonly Consumer _consumer = new Consumer();

        [GlobalSetup]
        public void GlobalSetup()
        {
            _dic = _update.ToDictionary(x => x.Value);
        }

        [Benchmark]
        public void InvalidateCollection()
        {
            _source.Invalidate(_update, _mergeFunc, x => x.Value);
            _source.Consume(_consumer);
        }

        [Benchmark]
        public void Merge()
        {
            var result = _source.Merge(_update, _mergeFunc, x => x.Value).ToList();
            result.Consume(_consumer);
        }

        [Benchmark]
        public void MergeWithoutMaterialize()
        {
            var result = _source.Merge(_update, _mergeFunc, x => x.Value);
            result.Consume(_consumer);
        }

        [Benchmark]
        public void MergeOnDictionaryWithoutMaterialize()
        {
            var result = _dic.Merge(_update, _mergeFunc, x => x.Value);
            result.Consume(_consumer);
        }
    }
}