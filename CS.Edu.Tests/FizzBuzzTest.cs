using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests
{

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

        //JS Example
        // for (let n = 1; n <= 100; n++) {
        //     let output = '';
        //     if (n % 3 === 0) output += 'Fizz';
        //     if (n % 5 === 0) output += 'Buzz';
        //     console.log(output || n);
        // }

        [Test]
        public void FizzBuzz1()
        {
            for (int i = 0; i <= 100; i++)
            {
                string output = string.Empty;
                if(i % 3 == 0) output += "Fizz";                
                if(i % 5 == 0) output += "Buzz";

                var result = string.IsNullOrEmpty(output) ? i.ToString() : output;

                Console.WriteLine(output);
            }
        }

        [Test]
        public void FizzBuzz2()
        {
            string Selector(int n)
            {
                return n switch
                {
                    int x when x % 15 == 0 => "FizzBuzz",
                    int x when x % 5 == 0 => "Buzz",
                    int x when x % 3 == 0 => "Fizz",
                    _ => n.ToString()
                };
            }

            Enumerable.Range(1, 101)
                .Select(x => Selector(x))
                .ForEach(x => Console.WriteLine(x));
        }

        [Test]
        public void FizzBuzz3()
        {
            //x => Check(3) ? Check(5) : ToString
        }

        [Test]
        public void FizzBuzz()
        {

            // var result = Enumerable.Range(1, 101)
            //     .Select(i => (i, s: i % 3 == 0 ?  "Fizz" : ""))
            //     .Select(x => (x.i, s: x.s += (x.i % 5 == 0 ?  "Buzz" : "")))
            //     .Select(x => string.IsNullOrEmpty(x.s) ? x.i.ToString() : x.s)
            //     .ToList();

            Assert.True(true);
        }
    }
}