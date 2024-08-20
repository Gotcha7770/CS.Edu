using System;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class StringReplaceTests
{
    [Theory]
    [InlineData("18-Янв-2024 12:31:38", "18-Jan-2024 12:31:38")]
    [InlineData("18-Июл-2024 12:31:38", "18-Jul-2024 12:31:38")]
    [InlineData("18-Дек-2024 12:31:38", "18-Dec-2024 12:31:38")]
    public void ReplaceTokenTest(string input, string expected)
    {
        ReplaceToken1(input)
            .Should()
            .Be(expected);
    }

    private static string ReplaceToken1(string input)
    {
        Range range = new(new Index(3), new Index(6));
        return input.AsSpan(range) switch
        {
            "Янв" => Replace(input, range, "Jan"),
            "Фев" => Replace(input, range, "Feb"),
            "Мар" => Replace(input, range, "Mar"),
            "Апр" => Replace(input, range, "Apr"),
            "Май" => Replace(input, range, "May"),
            "Июн" => Replace(input, range, "Jun"),
            "Июл" => Replace(input, range, "Jul"),
            "Авг" => Replace(input, range, "Aug"),
            "Сен" => Replace(input, range, "Sep"),
            "Окт" => Replace(input, range, "Oct"),
            "Ноя" => Replace(input, range, "Nov"),
            "Дек" => Replace(input, range, "Dec"),
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

    private static string ReplaceToken2(string input)
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
}