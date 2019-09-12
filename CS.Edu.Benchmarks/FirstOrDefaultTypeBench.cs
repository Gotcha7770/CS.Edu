using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks
{
    public class NumberType
    {
        public int Number { get; set; }
    }

    public class StringType
    {
        public string Value { get; set; }
    }

    [Config(typeof(DefaultConfig))]
    public class FirstOrDefaultTypeBench
    {
        private readonly object[] Items = Enumerable.Range(0, 1000)
            .Select(x => x == 500 ? (object)new NumberType { Number = x } : new StringType { Value = x.ToString() })
            .ToArray();

        [Benchmark]
        public object GetFirstOrDefaultOfType()
        {
            return Items.OfType<NumberType>().FirstOrDefault();
        }

        [Benchmark]
        public object GetFirstOrDefaultWhereTypeCheck()
        {
            return Items.FirstOrDefault(x => x is NumberType) as NumberType;
        }
    }
}
