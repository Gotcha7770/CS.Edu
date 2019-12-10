using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class LongExt
    {
        public static IEnumerable<long> Range(long start, long count, int step = 1)
        {
            ulong max = (ulong)start + (ulong)count - 1;
            if (count < 0 || max > long.MaxValue) throw new ArgumentOutOfRangeException("count");

            return RangeIterator(start, count, step);
        }

        static IEnumerable<long> RangeIterator(long start, long count, int step)
        {
            while(count >= 0 && start > 0)
            {
                yield return start;

                count--;
                start += step;
            }
        }
    }
}