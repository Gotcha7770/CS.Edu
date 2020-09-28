using System;

namespace CS.Edu.Tests.Utils
{
    class Groupable<T, TKey> : Valuable<T>
    {
        private TKey _groupKey;

        public Groupable(Guid key, T value, TKey groupKey)
            : base(key, value)
        {
            GroupKey = groupKey;
        }

        public Groupable(T value, TKey groupKey)
            : base(value)
        {
            GroupKey = groupKey;
        }

        public TKey GroupKey
        {
            get => _groupKey;
            set => SetAndRaise(ref _groupKey, value);
        }
    }
}