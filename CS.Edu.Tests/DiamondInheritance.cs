using System;
using Xunit;

namespace CS.Edu.Tests;

public class DiamondInheritance
{
    interface IA
    {
        void M();
    }

    interface IB : IA
    {
        //override void M() => Console.WriteLine("IB");
        void M() => Console.WriteLine("IB");
    }

    class Base : IA
    {
        void IA.M() => Console.WriteLine("Base");
    }

    class Derived : Base, IB { }

    [Fact]
    public void Test()
    {
        IA a = new Derived();
        a.M(); // what does it do?
    }
}