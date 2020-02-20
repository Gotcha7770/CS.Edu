using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CS.Edu.Core.Comparers
{
    public class GenericComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _compareFunc;

        public GenericComparer(Func<T, T, int> compareFunc)
        {
            _compareFunc = compareFunc;
        }

        public int Compare([AllowNull] T x, [AllowNull] T y)
        {
            return _compareFunc(x,y);
        }
    }
}