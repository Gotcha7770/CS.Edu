using BenchmarkDotNet.Attributes;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks.Extensions
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class RelationBench
    {
        Relation<int> relation1 = new Relation<int>((x, y) => (x, y) switch
            {
                (0, 0) => true,
                (0, _) => false,
                (_, 0) => false,
                _ => true
            });

        Relation<int> relation2 = new Relation<int>((x, y) => x == 0 && y == 0 || x != 0 && y != 0);

        Relation<int> relation3 = new Relation<int>((x, y) => x == 0 ? y == 0 : y != 0);

        [Benchmark]
        public bool[] Relation1()
        {
            return new bool[]{
                relation1(0, 0),
                relation1(21, 0),
                relation1(0, 21),
                relation1(21, 21)
            };
        }

        [Benchmark]
        public bool[] Relation2()
        {
            return new bool[]{
                relation2(0, 0),
                relation2(21, 0),
                relation2(0, 21),
                relation2(21, 21)
            };
        }

        [Benchmark]
        public bool[] Relation3()
        {
            return new bool[]{
                relation3(0, 0),
                relation3(21, 0),
                relation3(0, 21),
                relation3(21, 21)
            };
        }
    }
}