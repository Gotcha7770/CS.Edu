using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Extensions
{
    public static class LongExt
    {
        public static IEnumerable<long> Range(long start, long count, int step = 1)
        {
            long max = start + count;
            if (count < 0 || max < 0)
                throw new ArgumentOutOfRangeException("count");

            return (step) switch
            {
                1 => SimpleRangeIterator(start, count),
                _ => RangeIterator(start, count, step)
            };
        }

        static IEnumerable<long> SimpleRangeIterator(long start, long count)
        {
            for (long i = 0; i < count; i++)
            {
                yield return start++;
            }
        }

        static IEnumerable<long> RangeIterator(long start, long count, int step)
        {
            long max = start + step * count;
            if (max < 0)
                throw new InvalidOperationException("count and step parameters produce value out of range");

            for (long i = 0; i < count; i++)
            {
                yield return start;
                start += step;
            }
        }
    }
}