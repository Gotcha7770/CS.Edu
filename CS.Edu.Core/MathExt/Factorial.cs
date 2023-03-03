using System.Collections.Generic;

namespace CS.Edu.Core.MathExt;

public static class Factorial
{
    public static int Recursive(int n)
    {
        if (n == 0)
            return 1;

        return n * Recursive(n - 1);
    }

    public static IEnumerable<int> Iterator()
    {
        for (int i = 1, acc = 1 ;; i++)
        {
            yield return acc;
            acc *= i;
        }
    }
}