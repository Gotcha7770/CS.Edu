
using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class AggregateManyBench
    {        
        private readonly Random _random = new Random((int)DateTime.Now.Ticks);
        private IEnumerable<int> _items;

        static (int min, int max) GetMinMax1(IEnumerable<int> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            int min = 0, max = 0;
            bool flag = false;
            foreach (int d in source)
            {
                if (flag)
                {
                    if (d < min)
                        min = d;
                    if (d > max)
                        max = d;
                }
                else
                {
                    min = d;
                    max = d;
                    flag = true;
                }
            }
            if (flag)
                return (min, max);

            throw new InvalidOperationException("Source is empty");
        }

        static (int min, int max) GetMinMax2(IEnumerable<int> source)
        {
            int min = int.MaxValue;

            int max = source.Scan((_, curr) => {min = Math.Min(curr, min); return curr;})
                .Aggregate((acc, curr) => acc = Math.Max(curr, acc));

            return (min, max);
        }

        static (int min, int max) GetMinMax3(IEnumerable<int> source)
        {
            int min = int.MaxValue;

            int max = source.Do((x) => min = Math.Min(x, min))
                .Aggregate((acc, curr) => acc = Math.Max(curr, acc));

            return (min, max);
        }

        static (int min, int max) GetMinMax4(IEnumerable<int> source)
        {
            return source.Aggregate((min: int.MaxValue, max: int.MinValue), (acc, curr) => (Math.Min(curr, acc.min), Math.Max(curr, acc.max)));
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var array = new int[1000];
            for (int i = 0; i < 1000; i++)
            {
                array[i] = _random.Next(0, 5000);
            }

            _items = array;
        }

        [Benchmark]
        public (int min, int max) GetMinMaxBench()
        {
            return GetMinMax1(_items);
        }

        [Benchmark]
        public (int min, int max) ScanBench()
        {
            return GetMinMax2(_items);
        }

        [Benchmark]
        public (int min, int max) DoBench()
        {
            return GetMinMax3(_items);
        }

        [Benchmark]
        public (int min, int max) AggregateBench()
        {
            return GetMinMax4(_items);
        }
    }
}