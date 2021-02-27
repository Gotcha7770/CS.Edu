using System;

namespace CS.Edu.Tests.Utils
{
    class Groupable<T, TKey> : Valuable<T>
    {
        private TKey _groupKey;

        public Groupable(Guid key, T value, TKey groupKey) : base(key, value)
        {
            GroupKey = groupKey;
        }

        public Groupable(T value, TKey groupKey) : base(value)
        {
            GroupKey = groupKey;
        }

        public TKey GroupKey
        {
            get => _groupKey;
            set => SetAndRaise(ref _groupKey, value);
        }
    }

    class Groupable<T, TKey1, TKey2> : Valuable<T>
    {
        private TKey1 _groupKey1;
        private TKey2 _groupKey2;

        public Groupable(Guid key, T value, TKey1 groupKey1, TKey2 groupKey2) : base(key, value)
        {
            GroupKey1 = groupKey1;
            GroupKey2 = groupKey2;
        }

        public Groupable(T value, TKey1 groupKey1, TKey2 groupKey2) : base(value)
        {
            GroupKey1 = groupKey1;
            GroupKey2 = groupKey2;
        }

        public TKey1 GroupKey1
        {
            get => _groupKey1;
            set => SetAndRaise(ref _groupKey1, value);
        }

        public TKey2 GroupKey2
        {
            get => _groupKey2;
            set => SetAndRaise(ref _groupKey2, value);
        }
    }
}