using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class IntExt
    {
        public static long Power(this int x, int y)
        {
            return (long)Math.Pow(x, y);
        }        

        public static bool IsEven(this int number)
        {
            return (number & 1) == 0;
        }

        public static bool IsEven(this long number)
        {
            return (number & 1) == 0;
        }

        public static IEnumerable<long> Factorize(this long number)
        {
            number = Math.Abs(number);

            return (number) switch
            {
                0 => Enumerable.Empty<long>(),
                1 => EnumerableEx.Return(1L),
                _ => FactorizeIterator(number)
            };
        }

        static IEnumerable<long> FactorizeIterator(long number)
        {
            yield return 1;

            long constraint = (long)Math.Sqrt(number);
            long dividend = number;
            for (int i = 2; i <= constraint; i++)
            {
                while (dividend % i == 0)
                {
                    yield return i;
                    dividend /= i;
                }
            }

            if (dividend > 1)
                yield return dividend;
        }

        public static bool IsPrime(this long number)
        {
            number = Math.Abs(number);

            return number switch
            {
                0 | 1 => false,
                2 => true,
                _ => !(number.IsEven() || number.Factorize().Skip(2).Any())
            };
        }
    }
}
