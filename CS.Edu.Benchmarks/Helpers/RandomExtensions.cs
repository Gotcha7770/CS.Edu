using System;

namespace CS.Edu.Benchmarks.Helpers;

public static class RandomExtensions
{
    public static void Shuffle<T> (this Random random, T[] array)
    {
        int n = array.Length;
        while (n > 1)
        {
            int k = random.Next(n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
}