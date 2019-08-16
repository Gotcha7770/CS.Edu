using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks
{
    public enum ClassType
    {
        Type1,
        Type2
    }

    public interface ITyped
    {
        ClassType Type { get; }

        int Value { get; set; }
    }

    public class Type1 : ITyped
    {
        public ClassType Type => ClassType.Type1;

        public int Value { get; set; }
    }

    public class Type2 : ITyped
    {
        public ClassType Type => ClassType.Type2;

        public int Value { get; set; }

    }

    [Config(typeof(DefaultConfig))]
    public class FirstOrDefaultTypeBench
    {
        private readonly ITyped[] Items = Enumerable.Range(0, 1000)
            .Select(x => x.IsEven() ? (ITyped)new Type1 { Value = x } : new Type2 { Value = x })
            .ToArray();

        [Benchmark]
        public object GetFirstOrDefaultOfType()
        {
            return Items.OfType<Type1>().FirstOrDefault(x => x.Value == 500);
        }

        [Benchmark]
        public object GetFirstOrDefaultWhereFieldCheck()
        {
            return Items.FirstOrDefault(x => x.Type == ClassType.Type1 && x.Value == 500) as Type1;
        }

        [Benchmark]
        public object GetFirstOrDefaultWhereTypeCheck()
        {
            return Items.FirstOrDefault(x => x is Type1 && x.Value == 500) as Type1;
        }
    }
}
