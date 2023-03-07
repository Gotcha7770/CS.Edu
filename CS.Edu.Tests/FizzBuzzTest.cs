using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests;

[TestFixture]
public class FizzBuzzTest
{
    [Test]
    public void Devisibility()
    {
        var numbers = Enumerable.Range(1, 101)
            .Where(x => x % 3 == 0 && x % 5 == 0)
            .ToArray();

        Assert.That(numbers, Is.All.Matches<int>(x => x % 15 == 0));
    }

    [Test]
    public void FizzBuzz1()
    {
        for (int i = 0; i <= 100; i++)
        {
            string output = null;
            if (i % 3 == 0) output += "Fizz";
            if (i % 5 == 0) output += "Buzz";

            Console.WriteLine(output ?? i.ToString());
        }
    }

    [Test]
    public void FizzBuzz2()
    {
        Func<int, string> selector = (x) => (x % 3, x % 5) switch
        {
            (0, 0) => "FizzBuzz",
            (0, _) => "Buzz",
            (_, 0) => "Fizz",
            _ => x.ToString()
        };

        Enumerable.Range(1, 100)
            .Select(selector)
            .Do(x => Console.WriteLine(x))
            .ToList();
    }

    [Test]
    public void FizzBuzz3()
    {
        Enumerable.Range(1, 100)
            .Select(x => x % 15 == 0 ? "FizzBuzz"
                : x % 3 == 0 ? "Fizz" : x % 5 == 0 ? "Buzz" : x.ToString())
            .Do(x => Console.WriteLine(x))
            .ToList();
    }

    [Test]
    public void FizzBuzz4()
    {
        Enumerable.Range(1, 100)
            .Select(x => $"{(x % 3 * x % 5 == 0 ? 0 : x):#;}{x % 3:;;Fizz}{x % 5:;;Buzz}")
            .Do(x => Console.WriteLine(x))
            .ToList();
    }

    [Test]
    public void FizzBuzz5()
    {
        var combinations = new List<(Predicate<int> p, string s)>
        {
            (x => x % 3 == 0, "Fizz"),
            (x => x % 5 == 0, "Buzz"),
        };

        Enumerable.Range(1, 100)
            .Select(x => combinations.Where(c => c.p(x)).Select(x => x.s).DefaultIfEmpty(x.ToString()))
            .Select(x => string.Join("", x))
            .Do(x => Console.WriteLine(x))
            .ToList();
    }

    [Test]
    public void FizzBuzz6()
    {
        for (int i = 1; i <= 100; i++)
        {
            if (i % 3 == 0)
                Console.Write("Fizz");
            if (i % 5 == 0)
                Console.Write("Buzz");
            if (!(i % 3 == 0 || i % 5 == 0))
                Console.Write(i);

            Console.Write(Environment.NewLine);
        }
    }

    [Test]
    public void FizzBuzz7()
    {
        string[] fizzBuzzCycle = "FizzBuzz,{0},{0},Fizz,{0},Buzz,Fizz,{0},{0},Fizz,Buzz,{0},Fizz,{0},{0}"
            .Split(',');

        for (int i = 1; i <= 100; i++)
            Console.WriteLine(fizzBuzzCycle[i % fizzBuzzCycle.Length], i);
    }

    //x => Check(3) ? Check(5) : ToString
}