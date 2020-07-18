using System;

namespace Adalon.IO
{
    internal static class MathEx
    {
        private const ulong DeBruijnSequence64 = 0x03F79D71B4CB0A89;

#if NETCOREAPP

        private static ReadOnlySpan<byte> DeBruijnBitTable64 => new byte[]
#else
        private static readonly byte[] DeBruijnBitTable64 = 
#endif
        {
            0, 47, 1, 56, 48, 27, 2, 60,
            57, 49, 41, 37, 28, 16, 3, 61,
            54, 58, 35, 52, 50, 42, 21, 44,
            38, 32, 29, 23, 17, 11, 4, 62,
            46, 55, 26, 59, 40, 36, 15, 53,
            34, 51, 20, 43, 31, 22, 10, 45,
            25, 39, 14, 33, 19, 30, 9, 24,
            13, 18, 8, 12, 7, 6, 5, 63
        };

        

        internal static byte Log2(ulong value)
        {
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value |= value >> 32;
            return DeBruijnBitTable64[(int) ((value * DeBruijnSequence64) >> 58)];
        }
    }
}
