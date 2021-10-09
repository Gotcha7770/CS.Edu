using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using CS.Edu.Core.Extensions;
using EnumerableEx = System.Linq.EnumerableEx;

namespace CS.Edu.Tests
{
    [TestFixture]
    public class Exercises
    {
        //задача input превратить в output

        readonly string[] _input = new[]
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

        readonly string[] _output = new[]
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
        public void Imperative()
        {
            // var words = _input.Where(x => !x.IsNumber())
            //     .OrderBy(x => x)
            //     .ToArray();
            //
            // var numbers = _input.Where(x => x.IsNumber())
            //     .OrderByDescending(int.Parse)
            //     .ToArray();

            // for (int i = 0; i < _input.Length; i++)
            // {
            //     var item = _input[i];
            //     if (item.IsNumber())
            //     {
            //         numbers.Add(item);
            //     }
            // }

            // int n = 0;
            // int w = 0;
            string[] result = new string[_input.Length];

            for (int i = 0; i < _input.Length; i++)
            {
                var item = _input[i];
                //result[i] = item.IsNumber() ? numbers[n++] : words[w++];
            }

            Assert.AreEqual(_output, result);
        }

        [Test]
        public void Declarative()
        {
            // сортировать можно только имея все элементы на руках
            //(i, s, i) => i ? i[].Next : s.Next

            var result = EnumerableEx.Create<string>(async yielder =>
            {
                using (var control =  _input.Select(x => x.IsNumber()).GetEnumerator())
                using (var words = _input.Where(x => !x.IsNumber()).OrderBy(x => x).GetEnumerator())
                using (var numbers = _input.Where(x => x.IsNumber()).OrderByDescending(int.Parse).GetEnumerator())
                {
                    while (control.MoveNext())
                    {
                        var awaitable = control.Current switch
                        {
                            true when numbers.MoveNext() => yielder.Return(numbers.Current),
                            false when words.MoveNext() => yielder.Return(words.Current),
                            _ => yielder.Break()
                        };

                        await awaitable;
                    }
                }
            });

            Assert.That(result, Is.EqualTo(_output));
        }
    }
}