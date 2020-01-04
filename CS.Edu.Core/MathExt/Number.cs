using System.Collections.Generic;
using System.Linq;

namespace CS.Edu.Core.MathExt
{
    public static class Number
    {
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