using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Core.MathExt
{
    public static class PrimesGenerator
    {
        public static IEnumerable<long> GetPrimes()
        {
            return PrimesIterator();
        }

        private static IEnumerable<long> PrimesIterator()
        {
            yield return 2;
            yield return 3;

            long current = 5;
            int step = 0;
            var factors = new List<long> { 2, 3 };

            while (current < long.MaxValue)
            {
                if (factors.All(x => current % x != 0))
                {
                    factors.Add((int)current);
                    yield return current;
                }

                current += step.IsEven() ? 2 : 4;
                ++step;
            }
        }
    }    
}