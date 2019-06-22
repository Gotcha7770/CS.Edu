using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Edu.Core.Extensions
{
    public static class IntExt
    {
        public static bool IsEven(this int number)
        {
            return (number & 1) == 0;
        }

        public static int CountOfDigits(this int number, int @base)
        {
            if (@base < 0)
                throw new InvalidOperationException("Base should be greater then zero!");

            return 0;
        }
    }
}
