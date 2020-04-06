using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Comparers
{
    public class GenericComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _compareFunc;

        public GenericComparer(Func<T, T, int> compareFunc)
        {
            _compareFunc = compareFunc;
        }

        public int Compare(T x, T y)
        {
            return _compareFunc(x,y);
        }
    }
}