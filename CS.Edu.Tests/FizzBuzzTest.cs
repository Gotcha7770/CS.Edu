using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class FizzBuzzTest
{
    private readonly string[] _standard = new[]
    {
        "1",
        "2",
        "Fizz",
        "4",
        "Buzz",
        "Fizz",
        "7",
        "8",
        "Fizz",
        "Buzz",
        "11",
        "Fizz",
        "13",
        "14",
        "FizzBuzz",
        "16",
        "17",
        "Fizz",
        "19",
        "Buzz",
        "Fizz",
        "22",
        "23",
        "Fizz",
        "Buzz",
        "26",
        "Fizz",
        "28",
        "29",
        "FizzBuzz",
        "31",
        "32",
        "Fizz",
        "34",
        "Buzz",
        "Fizz",
        "37",
        "38",
        "Fizz",
        "Buzz",
        "41",
        "Fizz",
        "43",
        "44",
        "FizzBuzz",
        "46",
        "47",
        "Fizz",
        "49",
        "Buzz"
    };

    [Fact]
    public void Divisibility()
    {
        var numbers = Enumerable.Range(1, 101)
            .Where(x => x % 3 == 0 && x % 5 == 0)
            .ToArray();

        numbers.Should().OnlyContain(x => x % 15 == 0);
    }

    [Fact]
    public void FizzBuzz1()
    {
        string Selector(int x)
        {
            string output = null;
            if (x % 3 == 0) output += "Fizz";
            if (x % 5 == 0) output += "Buzz";

            return output ?? x.ToString();
        }

        var result = Enumerable.Range(1, 50).Select(Selector);
        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz2()
    {
        string Selector(int x) =>
            (x % 3, x % 5) switch
            {
                (0, 0) => "FizzBuzz",
                (0, _) => "Fizz",
                (_, 0) => "Buzz",
                _ => x.ToString()
            };

        var result = Enumerable.Range(1, 50).Select(Selector);
        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz3()
    {
        string Selector(int x)
        {
            return x % 15 == 0
                ? "FizzBuzz"
                : x % 3 == 0
                    ? "Fizz"
                    : x % 5 == 0
                        ? "Buzz"
                        : x.ToString();
        }
        var result = Enumerable.Range(1, 50).Select(Selector);
        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz4()
    {
        var result = Enumerable.Range(1, 50).Select(x => $"{(x % 3 * x % 5 == 0 ? 0 : x):#;}{x % 3:;;Fizz}{x % 5:;;Buzz}");
        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
    public void FizzBuzz5()
    {
        var combinations = new List<(Predicate<int> p, string s)>
        {
            (x => x % 3 == 0, "Fizz"),
            (x => x % 5 == 0, "Buzz"),
        };

        var result = Enumerable.Range(1, 50)
            .Select(x => combinations.Where(t => t.p(x))
                .Select(t => t.s)
                .DefaultIfEmpty(x.ToString()))
            .Select(x => string.Join("", x));
        result.Should().BeEquivalentTo(_standard);
    }

    [Fact]
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

    [Fact]
    public void FizzBuzz7()
    {
        string[] fizzBuzzCycle = "FizzBuzz,{0},{0},Fizz,{0},Buzz,Fizz,{0},{0},Fizz,Buzz,{0},Fizz,{0},{0}"
            .Split(',');

        string Selector(int x)
        {
            string template = fizzBuzzCycle[x % fizzBuzzCycle.Length];
            return string.Format(template, x);
        }

        var result = Enumerable.Range(1, 50).Select(Selector);
        result.Should().BeEquivalentTo(_standard);
    }
}