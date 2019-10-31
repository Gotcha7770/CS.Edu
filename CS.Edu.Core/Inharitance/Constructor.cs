using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace CS.Edu.Core.Inharitance
{
    interface IBase { }

    class Base : IBase { }

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

    public class Test
    {
        IEnumerable<Base> _items;
        Base[] _array;
        IContainer<Base> _container;
        List<Base> _list;

        public Test()
        {
            _items = new Collection<Derrived>();
            
            _array = new Derrived[10];
            _array[0] = new Derrived();

            _container = new TestContainer<Derrived>();

            //_list = new List<Derrived>(); //ERROR
            //IBase[] items = EnumerableEx.Return(new BaseStruct()).ToArray(); //ERROR
        }
    }
}
