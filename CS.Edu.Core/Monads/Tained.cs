using System;

struct Tainted<T>
{
    public T Value { get; }
    public bool IsTainted { get; }

    private Tainted(T value, bool isTainted)
    {
        Value = value;
        IsTainted = isTainted;
    }

    public static Tainted<R> Bind<A, R>(Tainted<A> tainted,
                                        Func<A, Tainted<R>> function)
    {
        Tainted<R> result = function(tainted.Value);        
        if (tainted.IsTainted && !result.IsTainted)
            return new Tainted<R>(result.Value, true);
        else
            return result;
    }
}