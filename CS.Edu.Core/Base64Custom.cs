using System;

namespace CS.Edu.Core;

public static class Base64Custom
{
    // RFC 4648: The "URL and Filename safe" Base 64 Alphabet
    private static ReadOnlySpan<char> EncodingMap => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-";

    // https://ru.wikipedia.org/wiki/Base64
    public static void EncodeToUtf8(Span<byte> bytes, Span<char> utf8)
    {
        // how many bytes after packs of 3
        int leftover = bytes.Length % 3;
        //ref char encodingMap = ref MemoryMarshal.GetReference(EncodingMap);
        int i = 0;
        int j = 0;
        while (i + 2 < bytes.Length)
        {
            var (a, b, c, d) = TripletToQuartet(bytes[i++], bytes[i++], bytes[i++]);
            // Unsafe.WriteUnaligned(bufferBytes + destinationIndex, result);
            utf8[j++] = EncodingMap[a];
            utf8[j++] = EncodingMap[b];
            utf8[j++] = EncodingMap[c];
            utf8[j++] = EncodingMap[d];
        }

        if (leftover == 2)
        {
            var (a, b, c, _) = TripletToQuartet(bytes[i++], bytes[i], 0);
            utf8[j++] = EncodingMap[a];
            utf8[j++] = EncodingMap[b];
            utf8[j] = EncodingMap[c];
        }
        else if (leftover == 1)
        {
            var (a, b, _, _) = TripletToQuartet(bytes[i], 0, 0);
            utf8[j++] = EncodingMap[a];
            utf8[j] = EncodingMap[b];
        }
    }

    private static (int a, int b, int c, int d) TripletToQuartet(int x, int y, int z)
    {
        var triplet = (x << 16) | (y << 8) | z;
        // uint i0 = Unsafe.Add(ref encodingMap, (IntPtr)(i >> 18));
        // uint i1 = Unsafe.Add(ref encodingMap, (IntPtr)((i >> 12) & 0x3F));
        // uint i2 = Unsafe.Add(ref encodingMap, (IntPtr)((i >> 6) & 0x3F));
        // uint i3 = Unsafe.Add(ref encodingMap, (IntPtr)(i & 0x3F));
        int a = (triplet & 0xFC0000) >> 18;
        int b = (triplet & 0x03F000) >> 12;
        int c = (triplet & 0x000FC0) >> 6;
        int d = triplet & 0x00003F;

        return (a, b, c, d);
    }
}