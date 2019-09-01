using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class BinarySplitBench
    {
        // Задача свернуть последовательность { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 }
        // до такой {{1, 3}, {0, 0, 0}, {7}, {0,0}, {9}, {0}, {1}} -> {1, 3, 0, 7, 0, 9, 0, 1},
        // то есть редуцировать все группы нулей до одного

        public IEnumerable<int> items = new[] { 1, 3, 0, 0, 0, 7, 0, 0, 9, 0, 1 };

        Func<int, bool> nonZero = x => x != 0;

        Relation<int> relation = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

        [Benchmark]
        public int[] SplitWithCycle()
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

            return result.ToArray();
        }

        [Benchmark]
        public int[] SplitWithLINQ()
        {
            static IEnumerable<TSource> SpecialIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> constraint)
            {
                IEnumerable<TSource> left = source;
                while (left.Any())
                {
                    foreach (TSource item in left.TakeWhile(constraint))
                    {
                        yield return item;
                    }

                    left = left.SkipWhile(constraint);
                    yield return left.FirstOrDefault();

                    left = left.SkipWhile(x => !constraint(x));
                }
            }

            return SpecialIterator(items, nonZero).ToArray();
        }

        [Benchmark]
        public int[] Split()
        {
            return items.Split(relation)
                .Select(x => x.Any(y => y == 0) ? Enumerable.Repeat(x.First(), 1) : x)
                .SelectMany(x => x)
                .ToArray();
        }

        internal IEnumerable<int> Reduce(IEnumerable<int> source)
        {
            int first = source.First();
            if (first == 0)
                yield return first;
            else
            {
                foreach (var item in source)
                    yield return item;
            }
        }

        [Benchmark]
        public int[] Split2()
        {
            return items.Split(relation)
                .Select(x => Reduce(x))
                .SelectMany(x => x)
                .ToArray();
        }

        public IEnumerable<IEnumerable<TSource>> SpecialSplit<TSource>(IEnumerable<TSource> source, Relation<TSource> relation)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (relation == null)
                throw new ArgumentNullException(nameof(relation));

            int countToSkip = 0;
            while (source.Skip(countToSkip).Any())
            {
                yield return TakeIterator(source, countToSkip, relation);                

                countToSkip += CounterIterator(source, countToSkip, relation);
            }
        }

        static IEnumerable<TSource> TakeIterator<TSource>(IEnumerable<TSource> source, int countToSkip, Relation<TSource> relation)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext()) countToSkip--;
                
                if (!enumerator.MoveNext())
                    yield break;

                TSource prev = enumerator.Current;
                yield return prev;

                while (enumerator.MoveNext())
                {
                    if (!relation(prev, enumerator.Current))
                        break;

                    prev = enumerator.Current;
                    yield return prev;
                }
            }
        }

        static int CounterIterator<TSource>(IEnumerable<TSource> source, int countToSkip, Relation<TSource> relation)
        {            
            using (var enumerator = source.GetEnumerator())
            {
                while (countToSkip > 0 && enumerator.MoveNext()) countToSkip--;

                if (!enumerator.MoveNext())
                    return 0;

                int result = 1;
                TSource prev = enumerator.Current;

                while(enumerator.MoveNext() && relation(prev, enumerator.Current))
                {
                    result++;
                    prev = enumerator.Current;
                }

                return result;
            }            
        }

        [Benchmark]
        public int[] Split3()
        {
            return SpecialSplit(items, relation)
                .Select(x => Reduce(x))
                .SelectMany(x => x)
                .ToArray();
        }
    }
}
