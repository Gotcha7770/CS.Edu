using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class ListBench
    {
        List<int> list = Enumerable.Range(0, 1000).ToList();
        LinkedList<int> linkedList = ToLinkedList(Enumerable.Range(0, 1000));

        static LinkedList<T> ToLinkedList<T>(IEnumerable<T> source)
        {
            return new LinkedList<T>(source);
        }

        [Benchmark]
        public List<int> ListWith1000Elements()
        {
            return Enumerable.Range(0, 1000).ToList();
        }

        [Benchmark]
        public List<int> ListWith1000_000_Elements()
        {
            return Enumerable.Range(0, 1000_000).ToList();
        }

        [Benchmark]
        public int ListConsuming()
        {
            return list.Sum();
        }

        [Benchmark]
        public LinkedList<int> LinkedListWith1000Elements()
        {
            return ToLinkedList(Enumerable.Range(0, 1000));
        }

        [Benchmark]
        public LinkedList<int> LinkedListWith1000_000_Elements()
        {
            return ToLinkedList(Enumerable.Range(0, 1000_000));
        }


        [Benchmark]
        public int LinkedListConsuming()
        {
            return linkedList.Sum();
        }
    }
}
