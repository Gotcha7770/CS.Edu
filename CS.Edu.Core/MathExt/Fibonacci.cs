using System.Collections.Generic;

namespace CS.Edu.Core.MathExt
{
    public static class Fibonacci
    {
        public static int Recursive(int n)
        {
            return GetNthFibonacci(0, 1, n);
        }

        static int GetNthFibonacci(int a, int b, int n)
        {
            return n > 1 ? GetNthFibonacci(b, b + a, --n) : b;
        }

        public static IEnumerable<int> Iterator()
        {
            for (int x = 0, y = 1 ;; y = x + y, x = y - x)
            {
                yield return x;
            }
        }
    }
}