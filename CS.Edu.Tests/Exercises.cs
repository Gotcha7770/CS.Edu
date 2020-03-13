using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NUnit.Framework;
using DynamicData;
using DynamicData.Binding;
using CS.Edu.Core.Extensions;
using CS.Edu.Core.Comparers;

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
        public void Interactive1()
        {
            var order = _input.Select(x => x.IsNumber() ? typeof(int) : typeof(string));

            var words = _input.Where(x => !x.IsNumber())
                .OrderBy(x => x)
                .ToArray();

            var numbers = _input.Where(x => x.IsNumber())
                .Select(x => int.Parse(x))
                .OrderByDescending(x => x)
                .Select(x => x.ToString())
                .ToArray();

            List<string> result = new List<string>();
            int i = 0;
            int j = 0;

            foreach (var item in order)
            {
                result.Add(item == typeof(int) ? numbers[j++] : words[i++]);
            }

            Assert.That(result, Is.EqualTo(_output));
        }

        [Test]
        public void Interactive2()
        {
            Func<string, string, int> compareFunc = (x, y) =>
            {
                return (x.IsNumber(), y.IsNumber()) switch
                {
                    (true, true) => int.Parse(x).CompareTo(int.Parse(y)) * -1,
                    _ => x.CompareTo(y)
                };
            };

            var comparer = new GenericComparer<string>(compareFunc);

            string[] result = _input.Copy();

            CollectionExt.PartialSort(result, Comparer<string>.Default, x => !x.IsNumber());
            CollectionExt.PartialSort(result, comparer, x => x.IsNumber());

            Assert.That(result, Is.EqualTo(_output));
        }


        [Test]
        public void Reactive()
        {
            var output = new ObservableCollectionExtended<string>();
            var changeSet = new SourceList<string>();

            // var order = changeSet
            //     .Connect()
            //     .Subscribe();

            // var subscribtion = changeSet
            //     .Connect()
            //     .Transform((x, i) => )
            //     .Bind(output)
            //     .Subscribe();


            //Assert.That(result, Is.EqualTo(_output));
        }
    }
}