using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks
{
    public class SomeType { }

    [Config(typeof(DefaultConfig))]
    public class FirstOrDefaultTypeBench
    {
        private readonly object[] Items = Enumerable.Range(0, 1000)
            .Select(x => new object())
            .ToArray();

        [GlobalSetup]
        public void GlobalSetup()
        {
            Items[499] = new SomeType();
        }

        [Benchmark]
        public object GetFirstOrDefaultOfType()
        {
            return Items.OfType<SomeType>().FirstOrDefault();
        }

        [Benchmark]
        public object GetFirstOrDefaultWhereTypeCheck()
        {
            return Items.FirstOrDefault(x => x is SomeType) as SomeType;
        }
    }
}
