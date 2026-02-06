using System;

namespace CS.Edu.Tests;

public class TheMostSpecificImplementationRule
{
    interface IA
    {
        void M() => Console.WriteLine("IA.M");
    }
    interface IB : IA
    {
        void IA.M() => Console.WriteLine("IB.M");
    }
    interface IC : IA
    {
        void IA.M() => Console.WriteLine("IC.M");
    }

    interface ID : IB, IC { } // compiles, but error when a class implements 'ID'

    // abstract class C : IB, IC { } // error: no most specific implementation for 'IA.M'

    abstract class D : IA, IB, IC // ok
    {
        public abstract void M();
    }

    // class E : ID { } // Error. No most specific implementation for 'IA.M'
}