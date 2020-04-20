using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using CS.Edu.Core.Extensions;

namespace CS.Edu.Benchmarks
{
    struct Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    [MemoryDiagnoser]
    [Config(typeof(DefaultConfig))]
    public class TestBench
    {
        private readonly Consumer _consumer = new Consumer();
        private readonly static Random Random = new Random();

        IEnumerable<Point> items = Enumerable.Range(0, 100)
            .Select(x => new Point { X = x, Y = -9999 })
            .Concat(Enumerable.Range(100, 10000).Select(GetRandomPoint))
            .Concat(Enumerable.Range(1100, 50).Select(x => new Point { X = x, Y = -9999 }));

        [Benchmark]
        public void SourceMethod()
        {
            var xValues = new List<double>();
            var yValues = new List<double>();

            var isMissingValue = false;
            foreach (var point in items)
            {
                if (!double.Equals(point.Y, -9999))
                {
                    isMissingValue = false;
                    xValues.Add(point.X);
                    yValues.Add(point.Y);
                }
                else if (!isMissingValue)
                {
                    isMissingValue = true;
                    xValues.Add(point.X);
                    yValues.Add(double.NaN);
                }
            }

            //обрезка NaN в начале и в конце
            RemoveCoordinateIfNanByIndex(xValues, yValues, 0);
            RemoveCoordinateIfNanByIndex(xValues, yValues, yValues.Count - 1);            

            xValues.Consume(_consumer);
            yValues.Consume(_consumer);
        }

        [Benchmark]
        public void ImprovedMethod()
        {
            var result = items.SkipWhile(x => Equals(x.Y, -9999))
                .ShrinkDuplicates(x => x.Y, -9999)
                .ExceptIfLast(x => x.Y, -9999)
                .Do(Replace);

            result.Consume(_consumer);
        }

        private void Replace(Point point)
        {
            if(Equals(point.Y, -9999))
                point.Y = double.NaN;
        }

        private static Point GetRandomPoint(int arg)
        {
            return new Point
            {
                X = arg,
                Y = Random.Next(0, 1) == 1 ? -9999 : -arg
            };
        }

        private void RemoveCoordinateIfNanByIndex(List<double> xValues, List<double> yValues, int index)
        {
            if (index >= 0 && yValues.Count > index && xValues.Count > index)
            {
                var y = yValues[index];
                if (double.IsNaN(y))
                {
                    yValues.RemoveAt(index);
                    xValues.RemoveAt(index);
                }
            }
        }
    }
}