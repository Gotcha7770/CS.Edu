using BenchmarkDotNet.Running;
using CS.Edu.Benchmarks.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CS.Edu.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<IsEvenBench>();
            Console.ReadKey();
        }
    }
}
