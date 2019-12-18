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
        public void FizzBuzz()
        {
            // for (let n = 1; n <= 100; n++) {
            //     let output = '';
            //     if (n % 3 === 0) output += 'Fizz';
            //     if (n % 5 === 0) output += 'Buzz';
            //     console.log(output || n);
            // }

            string Selector(int value)
            {
                return value switch
                {
                    int a when a % 3 == 0 => 1,
                    _ => 0
                };
            }

            // var result = Enumerable.Range(1, 101)
            //     .Select(i => (i, s: i % 3 == 0 ?  "Fizz" : ""))
            //     .Select(x => (x.i, s: x.s += (x.i % 5 == 0 ?  "Buzz" : "")))
            //     .Select(x => string.IsNullOrEmpty(x.s) ? x.i.ToString() : x.s)
            //     .ToList();

            var result = Enumerable.Range(1, 101)
                .Select(x => x % 3 == 0 ?  )
                .Select(x => (x.i, s: x.s += (x.i % 5 == 0 ?  "Buzz" : "")))
                .Select(x => string.IsNullOrEmpty(x.s) ? x.i.ToString() : x.s)
                .ToList();

            Assert.True(true);
        }
    }
}