using System;
using System.Linq;
using System.Reactive.Linq;
using NUnit.Framework;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class Exercises
    {
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

            var result = words.Zip(numbers, (l, r) => new [] {l, r.ToString()})
                .SelectMany(x => x);

            // var result = _input.GroupBy(x => keySelector(x))
            //     .Select(x => x.OrderBy(x => x));

            // Assert.That(result, Is.EqualTo(_output));
        }

        [Test]
        public void Reactive()
        {
            // _input.ToObservable()
            //     .Subscribe()

            //Assert.That(result, Is.EqualTo(_output));            
        }
    }
}