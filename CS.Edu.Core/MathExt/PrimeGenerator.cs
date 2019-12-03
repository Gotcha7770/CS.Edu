using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Core.MathExt
{    
    public static class PrimesGenerator
    {
        public static IEnumerable<R> Magic<T, R>(this IEnumerable<T> source,
                                                 Predicate<T> condition,
                                                 Func<T, R> transform,
                                                 Action<T> sideEffect)
        {
            return source.Select(x => condition(x) ? x : transform(x))
        }

        public static IEnumerable<long> GetPrimes()
        {
            return PrimesIterator();
        }

        private static IEnumerable<long> PrimesIterator2()
        {
            //var factors = new List<long> { 2 };
            
            Func<List<long>, long, long> func = (factors, num) => 
            {
                if(factors.All(x => num % x != 0)) //condition
                {
                    factors.Add(num); //side effect
                    return num; //result
                }

                return 0; //result
            };

            //var tmp = func.ApplyPartial(new List<long> { 2 });

            return EnumerableEx.Generate(3L, x => x < long.MaxValue, x => ++x, x => x)
                .Where(x => !x.IsEven())
                .Select(x => func.ApplyPartial(new List<long> { 2 })(x))
                .Where(x => x != 0)
                .Prepend(2);
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