using System;

namespace CS.Edu.Tests.Utils
{
    class Valuable<T> : Identity<Guid>
    {
        private T _value;

        public Valuable(Guid key, T value)
            : base(key)
        {
            Value = value;
        }

        public Valuable(T value)
            : base(Guid.NewGuid())
        {
            Value = value;
        }

        public T Value
        {
            get => _value;
            set => SetAndRaise(ref _value, value);
        }
    }
}