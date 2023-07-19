using System;
using System.Buffers.Text;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace CS.Edu.Tests;

public class Base64Tests
{
    [Theory]
    [InlineData(3917429, "dcY7")]
    [InlineData(3900673, "AYU7")]
    [InlineData(43900673, "Ad_dAg")]
    public void ToBase64UsingExample(long input, string output)
    {
        ConvertUsingExample(input)
            .Should()
            .Be(output);
    }

    [Theory]
    [InlineData(3917429, "dcY7")]
    [InlineData(3900673, "AYU7")]
    [InlineData(43900673, "Ad_dAg")]
    public void ToBase64UsingConverter(long input, string output)
    {
        ConvertUsingConverter(input)
            .Should()
            .Be(output);
    }

    [Theory]
    [InlineData(3917429, "dcY7")]
    [InlineData(3900673, "AYU7")]
    [InlineData(43900673, "Ad_dAg")]
    public void ToBase64UsingBase64(long input, string output)
    {
        ConvertUsingBase64(input)
            .Should()
            .Be(output);
    }

    private static string ConvertUsingExample(long input)
    {
        string actualTinyString = string.Empty;
        byte[] bytes = BitConverter.GetBytes(input);
        string base64 = Convert.ToBase64String(bytes);
        bool isPadding = true;
        for (int i = base64.Length-1; i >= 0; i--)
        {
            char c = base64[i];
            if (c == '=')
            {
                continue;
            }

            if (isPadding && c == 'A')
            {
                continue;
            }

            isPadding = false;
            if (c == '/')
            {
                actualTinyString += "-";
            }
            else if (c == '+')
            {
                actualTinyString = "_" + actualTinyString;
            }
            else if (c == '\n')
            {
                actualTinyString = "/" + actualTinyString;
            }
            else
            {
                actualTinyString = c + actualTinyString;
            }
        }

        return actualTinyString;
    }

    private static string ConvertUsingConverter(long input)
    {
        byte[] bytes = BitConverter.GetBytes(input);
        int firstZeroIndex = Array.IndexOf(bytes, (byte)0);
        string base64 = Convert.ToBase64String(bytes.AsSpan(0, firstZeroIndex));

        var sequence = base64
            .Replace('/', '-')
            .Replace('+', '_')
            .TakeWhile(x => x != '=');

        return new string(sequence.ToArray());
    }

    private static string ConvertUsingBase64(long input)
    {
        byte[] bytes = BitConverter.GetBytes(input);
        int firstZeroIndex = Array.IndexOf(bytes, (byte)0);
        Span<byte> utf8 = stackalloc byte[bytes.Length];
        var result = Base64.EncodeToUtf8(
            bytes.AsSpan(0, firstZeroIndex),
            utf8,
            out int consumed,
            out int written);

        Span<char> chars = stackalloc char[written];

        for (int i = 0; i < written; i++)
        {
            var c = Convert.ToChar(utf8[i]);
            chars[i] = c switch
            {
                '/' => '-',
                '+' => '_',
                _ => c
            };
        }

        int firstEqualsIndex = chars.IndexOf('=');
        return firstEqualsIndex > 0
            ? chars[..firstEqualsIndex].ToString()
            : chars.ToString();
    }
}