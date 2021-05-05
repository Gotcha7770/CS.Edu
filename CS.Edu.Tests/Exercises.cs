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
                .Select(int.Parse)
                .OrderByDescending(x => x)
                .Select(x => x.ToString())
                .ToArray();

            int i = 0;
            int j = 0;

            var result = order.Select(x => x == typeof(int) ? numbers[j++] : words[i++])
                .ToArray();

            Assert.AreEqual(_output, result);
        }

        [Test]
        public void Interactive2()
        {
            Comparison<string> comparsion = (x, y) =>
            {
                return (x.IsNumber(), y.IsNumber()) switch
                {
                    (true, true) => int.Parse(x).CompareTo(int.Parse(y)) * -1,
                    _ => string.Compare(x, y, StringComparison.Ordinal)
                };
            };

            var comparer = Comparer<string>.Create(comparsion);
            string[] result = _input.Copy();

            Collections.PartialSort(result, Comparer<string>.Default, x => !x.IsNumber());
            Collections.PartialSort(result, comparer, x => x.IsNumber());

            Assert.AreEqual(_output, result);
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