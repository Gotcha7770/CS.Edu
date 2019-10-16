using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CS.Edu.Core.Inharitance
{
    public class Base
    {
        public Base() { }

        public Base(int value) { }
    }

    public class Derrived : Base
    {
        public Derrived() { }

        public Derrived(int value) : base(value)
        {

        }
    }

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
            _list = new List<Base>();
        }
    }
}
