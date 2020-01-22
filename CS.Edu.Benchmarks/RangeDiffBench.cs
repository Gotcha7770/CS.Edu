using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class RangeDiffBench
    {
        public Range<int> A { get; } = new Range<int>(5, 9);

        [ParamsSource(nameof(Bs))]
        public Range<int> B { get; set; }

        public IEnumerable<Range<int>> Bs => new[] 
        {
            new Range<int>(0, 4),
            new Range<int>(0, 6),            
            new Range<int>(5, 9),
            new Range<int>(4, 10),
            new Range<int>(6, 8),
            new Range<int>(8, 10),
            new Range<int>(10, 11),
        };

        private Range<int>[] SubstructFromCenter(Range<int> one, Range<int> other)
        {
            return new[]
            {
                new Range<int>(one.Min, other.Min),
                new Range<int>(other.Max, one.Max)
            };
        }
        
        [Benchmark]
        public Range<int>[] CustomSubstruct()
        {
            bool left = A.Contains(B.Min);
            bool right = A.Contains(B.Max);

            return (left, right) switch
            {
                (true, true) => Equals(B) ? Array.Empty<Range<int>>() : SubstructFromCenter(A, B),
                (true, false) => new[] { new Range<int>(A.Min, B.Min) },
                (false, true) => new[] { new Range<int>(B.Max, A.Max) },
                _ => B.Contains(A) ? Array.Empty<Range<int>>() : new [] { A }
            };
        }

        [Benchmark]
        public Range<int>[] Substruct()
        {
            return A.Substruct(B).ToArray();
        }
    }
}