using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.Extensions
{
    public static class IntExt
    {
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
            //if number == 0
            number = System.Math.Abs(number);

            yield return 1;
            if (number == 1)
                yield break;

            long constraint = (long)System.Math.Sqrt(number);
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
            number = System.Math.Abs(number);

            if (number == 2)
                return true;
            else
                return number > 1
                       && !number.IsEven()
                       && !number.Factorize().Skip(2).Any();
        }
    }
}
