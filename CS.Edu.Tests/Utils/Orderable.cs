using System;

namespace CS.Edu.Tests.Utils;

class Orderable<T> : Valuable<T>
{
    private int _order;

    public Orderable(Guid key, T value, int order)
        :base(key, value)
    {
        Order = order;
    }

    public Orderable(T value, int order)
        :base(value)
    {
        Order = order;
    }

    public Orderable(int order)
        :base(default)
    {
        Order = order;
    }

    public int Order
    {
        get => _order;
        set => SetAndRaise(ref _order, value);
    }
}