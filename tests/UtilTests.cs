using NUnit.Framework;

namespace Adalon.IO.Tests
{    
    public class UtilTests
    {
        // test cases from
        // https://github.com/dotnet/runtime/blob/master/src/libraries/System.Runtime.Extensions/tests/System/Numerics/BitOperationsTests.cs
        // BitOperationsTests.BitOps_Log2_ulong
        [TestCase(0ul, 0)]
        [TestCase(1ul, 0)]
        [TestCase(2ul, 1)]
        [TestCase(3ul, 2 - 1)]
        [TestCase(4ul, 2)]
        [TestCase(5ul, 3 - 1)]
        [TestCase(6ul, 3 - 1)]
        [TestCase(7ul, 3 - 1)]
        [TestCase(8ul, 3)]
        [TestCase(9ul, 4 - 1)]
        [TestCase(byte.MaxValue, 8 - 1)]
        [TestCase(ushort.MaxValue, 16 - 1)]
        [TestCase(uint.MaxValue, 32 - 1)]
        [TestCase(ulong.MaxValue, 64 - 1)]
        // additional cases
        [TestCase(118ul, 6)]
        [TestCase(42ul, 5)]
        [TestCase(999ul, 9)]
        [TestCase(1000ul, 9)]
        [TestCase(1024ul, 10)]
        [TestCase(1080ul, 10)]
        [TestCase(12000ul, 13)]
        public void Log2(ulong value, int expected)
        {
            Assert.AreEqual((byte)expected,MathEx.Log2((ulong)value));
        }
    }
}
