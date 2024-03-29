using System;
using System.Linq;

namespace CS.Edu.Core.MathExt;

public static class Matrix
{
    public static T[,] To2D<T>(this T[][] source)
    {
        try
        {
            int FirstDim = source.Length;
            // ↓ throws InvalidOperationException if source is not rectangular
            int SecondDim = source.GroupBy(row => row.Length).Single().Key;

            var result = new T[FirstDim, SecondDim];
            for (int i = 0; i < FirstDim; ++i)
            for (int j = 0; j < SecondDim; ++j)
                result[i, j] = source[i][j];

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("The given jagged array is not rectangular.");
        }
    }
}