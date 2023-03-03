using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CS.Edu.Core.Inharitance;

interface IBase { }

class Base : IBase
{
    private readonly int _value;

    public Base() { }

    public Base(int value)
    {
        _value = value;
    }
}

struct BaseStruct : IBase { }

class Derrived : Base { }

interface IContainer<out T>
{
    T Value { get; }
}

class TestContainer<T> : IContainer<T>
{
    public T Value { get; }
}

class Test
{
    IEnumerable<Base> _items;
    Base[] _array;
    IContainer<Base> _container;
    List<Base> _list;

    Test()
    {
        _items = new Collection<Derrived>();

        _array = new Derrived[10];
        _array[0] = new Derrived();

        _container = new TestContainer<Derrived>();

        //_list = new List<Derrived>(); //ERROR
        //IBase[] items = EnumerableEx.Return(new BaseStruct()).ToArray(); //ERROR
    }
}