using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Benchmarks.Extensions
{
    [Config(typeof(DefaultConfig))]
    public class PaginateBench
    {
        public IEnumerable<int> items = Enumerable.Range(0, 100);

        //[Benchmark]
        //public void TunedPaginate()
        //{
        //    TunedPaginateIterator(items, 10).ToArray();
        //}

        [Benchmark]
        public void PrettyPaginate()
        {
            PrettyPaginateIterator(items, 10).ToArray();
        }


        //public IEnumerable<IEnumerable<TSource>> TunedPaginateIterator<TSource>(IEnumerable<TSource> source, int pageSize)
        //{
        //    bool guard = true;

        //    IEnumerable<TSource> InnerIterator(IEnumerator<TSource> e, int c)
        //    {
        //        int count = 0;
        //        while (e.MoveNext() && count <= c)
        //        {
        //            count++;
        //            yield return e.Current;
        //        }

        //        guard = count <= c;
        //    }

        //    using (IEnumerator<TSource> e = source.GetEnumerator())
        //    {

        //        while (guard)
        //        {
        //            yield return InnerIterator(e, pageSize);
        //        }
        //    };
        //}

        public IEnumerable<IEnumerable<TSource>> PrettyPaginateIterator<TSource>(IEnumerable<TSource> source, int pageSize)
        {
            int behind = 0;
            IEnumerable<TSource> left = source;
            while (left.Any())
            {
                yield return left.Take(pageSize);
                behind += pageSize;
                left = source.Skip(behind);
            }
        }
    }
}
