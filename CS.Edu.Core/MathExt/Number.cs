using System;
using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.MathExt
{
    public static class Number
    {
        public static long PowerOfTwo(int pow)
        {
            if(pow == 0)
                return 1;

            long @base = 2;
            for (int i = 1; i < pow; i++)
            {
                 @base <<= 1;
            }

            return @base;
        }

        public static int[] Digits(int number, int @base)
        {
            return DigitsIterator(number, @base).ToArray();
        }

        public static IEnumerable<int> DigitsIterator(int number, int @base)
        {
            do
            {
                yield return number % @base;
                number /= @base;
            } while (number > 0);
        }
    }
}