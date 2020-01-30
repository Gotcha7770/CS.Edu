using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class InvalidateCollectionBench
    {
        List<(int, DateTime)> _source = new List<(int, DateTime)>()
        {
            (0, new DateTime(2012, 2, 1)),
            (10, new DateTime(2013, 3, 1)),
            (20, new DateTime(2014, 5, 1)),
            (30, new DateTime(2015, 7, 1)),
            (40, new DateTime(2016, 11, 1))
        };

        (int, DateTime)[] _update = new[]
        {
            (0, new DateTime(2012, 2, 2)),
            (12, new DateTime(2013, 3, 3)),
            (22, new DateTime(2014, 4, 4)),
            (30, new DateTime(2015, 5, 2))
        };

        Dictionary<int, (int, DateTime)> _dic;

        readonly Merge<(int, DateTime)> _mergeFunc = (x, y) => (x.Item1, y.Item2);
        readonly Func<(int, DateTime), int> _keySelector = (x) => x.Item1;
        readonly Consumer _consumer = new Consumer();

        [GlobalSetup]
        public void GlobalSetup()
        {
            _dic = _update.ToDictionary(_keySelector);
        }

        [Benchmark]
        public void InvalidateCollection()
        {
            _source.Invalidate(_update, _mergeFunc, _keySelector);
            _source.Consume(_consumer);
        }

        [Benchmark]
        public void Merge()
        {
            var result = _source.Merge(_update, _mergeFunc, _keySelector).ToList();
            result.Consume(_consumer);
        }

        [Benchmark]
        public void MergeWithoutMaterialzie()
        {
            var result = _source.Merge(_update, _mergeFunc, _keySelector);
            result.Consume(_consumer);
        }

        [Benchmark]
        public void MergeOnDictionaryWithoutMaterialzie()
        {
            var result = _dic.Merge(_update, _mergeFunc, _keySelector);
            result.Consume(_consumer);
        }
    }
}