using System.Collections.Generic;

namespace CS.Edu.Core.Collections;

public class Map<T1, T2>
{
    private readonly Dictionary<T1, T2> _forward = new();
    private readonly Dictionary<T2, T1> _reverse = new();

    public Map()
    {
        Forward = new Indexer<T1, T2>(_forward);
        Reverse = new Indexer<T2, T1>(_reverse);
    }

    public void Add(T1 t1, T2 t2)
    {
        _forward.Add(t1, t2);
        _reverse.Add(t2, t1);
    }

    public Indexer<T1, T2> Forward { get; }
    public Indexer<T2, T1> Reverse { get; }
}

public class Indexer<T1, T2>
{
    private readonly Dictionary<T1, T2> _dictionary;

    public Indexer(Dictionary<T1, T2> dictionary)
    {
        _dictionary = dictionary;
    }

    public T2 this[T1 index]
    {
        get { return _dictionary[index]; }
    }
}