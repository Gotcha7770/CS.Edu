using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Edu.Core.Inharitance
{
    public class Base
    {
        public Base()
        {
                
        }

        public Base(int value)
        {

        }
    }

    public class Derrived : Base
    {
        public Derrived()
        {

        }

        public Derrived(int value) : base(value)
        {

        }
    }

    public interface ITest
    {
        Base[] Array { get; set; }
    }
    public class Test : ITest
    {
        Base[] _array;

        public Test()
        {
            _array = new Derrived[0];
        }

        public Base[] Array
        {
            get => _array;
            set { _array = value; }
        }
    }
}
