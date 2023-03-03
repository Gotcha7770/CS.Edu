using System.Collections;
using System.Collections.Generic;

namespace CS.Edu.Core.Helpers;

public static class Enumerator
{
    public static IEnumerator<T> Create<T>()
    {
        return new Enumerator<T>();
    }

    public static IEnumerator<T> Create<T>(T first) => new Enumerator<T>(first);

    public static IEnumerator<T> Create<T>(T first, T second) => new Enumerator<T>(first, second);

    public static IEnumerator<T> Create<T>(T first, T second, T third) => new Enumerator<T>(first, second, third);

    public static IEnumerator<T> Create<T>(T first, T second, T third, T forth)
    {
        return new Enumerator<T>(first, second, third, forth);
    }

    public static IEnumerator<T> Create<T>(T first, T second, T third, T forth, T fifth)
    {
        return new Enumerator<T>(first, second, third, forth, fifth);
    }
}

internal class Enumerator<T> : IEnumerator<T>
{
    private readonly int _length;
    private readonly T _first;
    private readonly T _second;
    private readonly T _third;
    private readonly T _forth;
    private readonly T _fifth;

    private int _state;

    internal Enumerator()
    { }

    internal Enumerator(T first, int length = 1)
    {
        _first = first;
        _length = length;
    }

    internal Enumerator(T first, T second, int length = 2)
        : this(first, length)
    {
        _second = second;
    }

    internal Enumerator(T first, T second, T third, int length = 3)
        : this(first, second, length)
    {
        _third = third;
    }

    internal Enumerator(T first, T second, T third, T forth, int length = 4)
        : this(first, second, third, length)
    {
        _forth = forth;
    }

    internal Enumerator(T first, T second, T third, T forth, T fifth, int length = 5)
        : this(first, second, third, forth, length)
    {
        _fifth = fifth;
    }

    public bool MoveNext()
    {
        if (_state >= _length)
            return false;

        _state++;
        return true;
    }

    public void Reset()
    {
        _state = 0;
    }

    public T Current => _state switch
    {
        1 => _first,
        2 => _second,
        3 => _third,
        4 => _forth,
        5 => _fifth
    };

    object IEnumerator.Current => Current;

    public void Dispose()
    { }
}