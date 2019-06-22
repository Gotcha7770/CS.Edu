using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;

namespace CS.Edu.Benchmarks
{
    public class DefaultConfig : ManualConfig
    {
        public DefaultConfig()
        {
            Add(StatisticColumn.Median);
            Add(StatisticColumn.Max);
        }
    }
}
