using System;

namespace CS.Edu.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void CopyPart<T>(this T[,] source, T[,] target, int row, int column, int implementation)
        {
            if(source.GetLength(0) < target.GetLength(0)
               || source.GetLength(1) < target.GetLength(1))
                throw new ArgumentException($"dimensions of {nameof(source)} should be grater then dimensions of {nameof(target)}");

            switch (implementation)
            {
                case 1:
                    Implementation2(source, target, row, column);
                    break;
                case 2:
                    Implementation2(source, target, row, column);
                    break;
                case 3:
                    Implementation3(source, target, row, column);
                    break;
            }
        }

        private static void Implementation1<T>(T[,] source, T[,] target, int row, int column)
        {
            for (int i = 0; i < target.GetLength(0); i++)
            for (int j = 0; j < target.GetLength(1); j++)
            {
                target[i, j] = source[i + row, j + column];
            }
        }

        private static void Implementation2<T>(T[,] source, T[,] target, int row, int column)
        {
            int sourceLength = source.GetLength(0);
            int targetLength = target.GetLength(0);
            for (int i = 0; i < target.GetLength(0); i++)
            {
                int sourceOffset = (row + i) * sourceLength + column;
                int targetOffset = i * targetLength;
                Array.Copy(source, sourceOffset, target, targetOffset,  targetLength);
            }
        }

        private static void Implementation3<T>(T[,] source, T[,] target, int row, int column)
        {
            int sourceLength = source.GetLength(0);
            int targetLength = target.GetLength(0);
        }
        //Buffer.BlockCopy(source, 5, target, 0, 4 * sizeof(int));
    }
}