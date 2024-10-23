using System;
using System.Collections.Generic;

namespace CS.Edu.Core.Comparers;

public static class EqualityComparer
{
    public static IEqualityComparer<TValue> Create<TValue, TKey>(Func<TValue, TKey> keySelector)
    {
        return new GenericEqualityComparer<TValue, TKey>(keySelector);
    }

    private class GenericEqualityComparer<TValue, TKey> : IEqualityComparer<TValue>
    {
        private readonly Func<TValue, TValue, bool> _equalsFunction;
        private readonly Func<TValue, int> _hashCodeFunction;

        public GenericEqualityComparer(Func<TValue, TKey> keySelector)
        {
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            _equalsFunction = (x, y) =>
            {
                TKey one = keySelector(x);
                TKey other = keySelector(y);

                return Equals(one, other);
            };

            _hashCodeFunction = x => keySelector(x).GetHashCode();
        }

        public bool Equals(TValue x, TValue y)
        {
            return _equalsFunction(x,y);
        }

        public int GetHashCode(TValue obj)
        {
            return _hashCodeFunction(obj);
        }
    }
}