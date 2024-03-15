using System;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class StringReplaceBench
{
    [Benchmark]
    public string ReplaceTokenWithStringMethod()
    {
        return ReplaceToken("18-Июл-2024 12:31:38");
    }

    [Benchmark]
    public string ReplaceTokenWithSpans()
    {
        return ReplaceHelper.ReplaceToken("18-Июл-2024 12:31:38");
    }

    private static string ReplaceToken(string input)
    {
        return input.Replace("Янв", "Jan")
            .Replace("Фев", "Feb")
            .Replace("Мар", "Mar")
            .Replace("Апр", "Apr")
            .Replace("Май", "May")
            .Replace("Июн", "Jun")
            .Replace("Июл", "Jul")
            .Replace("Авг", "Aug")
            .Replace("Сен", "Sep")
            .Replace("Окт", "Oct")
            .Replace("Ноя", "Nov")
            .Replace("Дек", "Dec");
    }

    private static class ReplaceHelper
    {
        private static readonly Range Range = new Range(new Index(3), new Index(6));
        public static string ReplaceToken(string input)
        {
            return input.AsSpan(Range) switch
            {
                "Янв" => Replace(input, Range, "Jan"),
                "Фев" => Replace(input, Range, "Feb"),
                "Мар" => Replace(input, Range, "Mar"),
                "Апр" => Replace(input, Range, "Apr"),
                "Май" => Replace(input, Range, "May"),
                "Июн" => Replace(input, Range, "Jun"),
                "Июл" => Replace(input, Range, "Jul"),
                "Авг" => Replace(input, Range, "Aug"),
                "Сен" => Replace(input, Range, "Sep"),
                "Окт" => Replace(input, Range, "Oct"),
                "Ноя" => Replace(input, Range, "Nov"),
                "Дек" => Replace(input, Range, "Dec"),
                _ => throw new FormatException()
            };
        }

        private static string Replace(ReadOnlySpan<char> input, Range range, ReadOnlySpan<char> newValue)
        {
            Span<char> chars = stackalloc char[input.Length];
            input.CopyTo(chars);
            newValue.CopyTo(chars[range]);

            return new string(chars);
        }
    }
}