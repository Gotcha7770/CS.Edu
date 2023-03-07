using System;

namespace CS.Edu.Tests.Utils;

class Valuable<T, TKey> : Identity<TKey>
{
    private T _value;

    public Valuable(TKey key, T value) : base(key) => Value = value;

    public T Value
    {
        get => _value;
        set => SetAndRaise(ref _value, value);
    }
}

static class Valuable
{
    public static Valuable<T> From<T>(T value) => new Valuable<T>(value);
}

class Valuable<T> : Valuable<T, Guid>
{
    public Valuable(T value) : base(Guid.NewGuid(), value) { }

    public Valuable(Guid key, T value) : base(key, value) { }
}