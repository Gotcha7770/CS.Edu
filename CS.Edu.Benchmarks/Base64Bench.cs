using System;
using System.Buffers.Text;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class Base64Bench
{
    [Params(1000000, 3917429, 3900673, 10000000, 43900673)]
    public long Input { get; set; }

    [Benchmark]
    public string ToBase64UsingExample()
    {
        return ConvertUsingExample(Input);
    }

    [Benchmark]
    public string ToBase64UsingConverter()
    {
        return ConvertUsingConverter(Input);
    }

    [Benchmark]
    public string ConvertUsingBase64()
    {
        return ConvertUsingBase64(Input);
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
       //int firstZeroIndex = Array.IndexOf(bytes, (byte)0);
        //string base64 = Convert.ToBase64String(bytes.AsSpan(0, firstZeroIndex));
        string base64 = Convert.ToBase64String(bytes);

        var sequence = base64
            .Replace('/', '-')
            .Replace('d', 's')
            .Replace('+', '_')
            .TakeWhile(x => x != '=');

        return new string(sequence.ToArray());
    }

    private static string ConvertUsingBase64(long input)
    {
        byte[] bytes = BitConverter.GetBytes(input);
        int firstZeroIndex = Array.IndexOf(bytes, (byte)0);
        Span<byte> utf8 = stackalloc byte[bytes.Length];
        Base64.EncodeToUtf8(
            bytes.AsSpan(0, firstZeroIndex),
            utf8,
            out int _,
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