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
    public class BinarySplitBench
    {
        // Задача свернуть последовательность типа { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 }
        // до такой {{1, 3}, {0, 0, 0}, {7}, {0,0}, {9}, {0}, {1}} -> {1, 3, 0, 7, 0, 9, 0, 1},
        // то есть редуцировать все группы нулей до одного

        private readonly Random _random = new Random((int)DateTime.Now.Ticks);

        private readonly Consumer _consumer = new Consumer();

        public IEnumerable<int> Items;

        Relation<int> bothAreZeroOrNot = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

        [GlobalSetup]
        public void GlobalSetup()
        {
            Items = Enumerable.Range(0, 1000)
                .Select(x => _random.Next(0, 2) == 0 ? _random.Next(1, 10) : 0)
                .ToArray();
        }

        [Benchmark]
        public void SplitWithCycle()
        {
            List<int> result = new List<int>();

            bool isZero = false;
            foreach (var item in Items)
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

            result.Consume(_consumer);
        }

        static IEnumerable<IEnumerable<T>> PlainSplitIterator<T>(IEnumerable<T> source, Relation<T> relation)
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
            var result = PlainSplitIterator(Items, bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            result.Consume(_consumer);
        }

        static IEnumerable<IEnumerable<T>> PlainSplitIterator2<T>(IEnumerable<T> source, Relation<T> relation)
        {
            List<T> acc;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                acc = new List<T> { enumerator.Current };

                while (enumerator.MoveNext())
                {
                    T item = enumerator.Current;
                    if (relation(acc.Last(), item))
                    {
                        acc.Add(item);
                    }
                    else
                    {
                        yield return acc;
                        acc = new List<T> { item };
                    }
                }
            }

            if (acc.Count > 0)
                yield return acc;
        }

        [Benchmark]
        public void PlainSplit2()
        {
            var result = PlainSplitIterator2(Items, bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            result.Consume(_consumer);
        }

        [Benchmark]
        public void Split()
        {
            var result = Items.Split(bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            result.Consume(_consumer);
        }

        static IEnumerable<IEnumerable<T>> IxSplitIterator<T>(IEnumerable<T> source, Relation<T> relation)
        {
            yield return Enumerable.Empty<T>();
        }

        //[Benchmark]
        public void IxSplit()
        {
            var result = IxSplitIterator(Items, bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            result.Consume(_consumer);
        }

        static IEnumerable<IEnumerable<T>> RxSplitIterator<T>(IEnumerable<T> source, Relation<T> relation)
        {
            yield return Enumerable.Empty<T>();
        }

        //[Benchmark]
        public void RxSplit()
        {
            var result = RxSplitIterator(Items, bothAreZeroOrNot)
                .SelectMany(x => x.First() == 0 ? EnumerableEx.Return(0) : x);

            result.Consume(_consumer);
        }
    }
}
