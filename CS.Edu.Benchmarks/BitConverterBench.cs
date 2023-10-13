using System;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace CS.Edu.Benchmarks;

[MemoryDiagnoser]
[Config(typeof(DefaultConfig))]
public class BitConverterBench
{
    [Params(1000000, 3917429, 3900673, 10000000, 43900673)]
    public long Input { get; set; }

    [Benchmark]
    public byte ToByteArrayUsingBitConverter()
    {
        return GetFirstByte(Input);
    }

    [Benchmark]
    public byte ToByteArrayCustom()
    {
        return GetFirstByteCustom(Input);
    }

    private static byte GetFirstByte(long input)
    {
        return BitConverter.GetBytes(input)[0];
    }

    private static byte GetFirstByteCustom(long input)
    {
        Span<byte> bytes = stackalloc byte[sizeof(long)];
        Unsafe.As<byte, long>(ref bytes[0]) = input;
        return bytes[0];
    }
}