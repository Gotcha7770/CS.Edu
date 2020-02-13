using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NUnit.Framework;
using DynamicData;
using DynamicData.Binding;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class Exercises
    {
        //задача input превратить в output
        
        string[] _input = new[]
        {
            "Вишня",
            "1",
            "Боб",
            "3",
            "Яблоко",
            "22",
            "0",
            "Арбуз"
        };

        string[] _output = new[]
        {
            "Арбуз",
            "22",
            "Боб",
            "3",
            "Вишня",
            "1",
            "0",
            "Яблоко"
        };

        [Test]
        public void Interactive()
        {
            var words = _input.Where(x => x.Any(c => char.IsLetter(c)))
                .OrderBy(x => x);

            var numbers = _input.Where(x => x.Any(c => char.IsNumber(c)))
                .Select(x => int.Parse(x))
                .OrderByDescending(x => x);

            var result = words.Zip(numbers, (l, r) => new[] { l, r.ToString() })
                .SelectMany(x => x);

            // Assert.That(result, Is.EqualTo(_output));
        }

        [Test]
        public void Interactive2()
        {
            Predicate<string> isNumber = (x) => x.All(c => char.IsNumber(c));

            var order = _input.Select(x => isNumber(x) ? typeof(int) : typeof(string));

            var words = _input.Where(x => !isNumber(x))
                .OrderBy(x => x)
                .ToArray();

            var numbers = _input.Where(x => isNumber(x))
                .Select(x => int.Parse(x))
                .OrderByDescending(x => x)
                .Select(x => x.ToString())
                .ToArray();

            List<string> result = new List<string>();
            int i = 0;
            int j = 0;

            foreach(var item in order)
            {
                result.Add(item == typeof(int) ? numbers[j++] : words[i++]);
            }
            
            Assert.That(result, Is.EqualTo(_output));
        }

        [Test]
        public void Reactive()
        {
            // var output = new List<string>();

            // var changeSet = new SourceList<string>();
            // var subscribtion = changeSet
            //     .Connect()
            //     .Transform(x => int.TryParse(x, out int i) ? (object)i : x)
            //     .GroupOn(x => x.GetType())
            //     .Transform(x => x.GroupKey == typeof(int) 
            //         ? x.)
            //     .Subscribe(x => output.Add(x));
            

            //Assert.That(result, Is.EqualTo(_output));
        }
    }
}