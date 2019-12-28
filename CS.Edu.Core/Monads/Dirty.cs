using System;

struct Dirty<T>
{
    public T Value { get; }
    public bool IsDirty { get; }

    private Dirty(T value, bool isDirty)
    {
        Value = value;
        IsDirty = isDirty;
    }

    public static Dirty<R> Bind<A, R>(Dirty<A> tainted, Func<A, Dirty<R>> function)
    {
        Dirty<R> result = function(tainted.Value);
        return (tainted.IsDirty, result.IsDirty) switch
        {
            (true, false) => new Dirty<R>(result.Value, true),
            _ => result
        };
    }
}