using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adalon.IO.Tests
{
    [TestClass]
    public class UtilTests
    {
        [DataRow(0L, 0)]
        [DataRow(118L, 6)]
        [DataRow(42L, 5)]
        [DataRow(999L, 9)]
        [DataRow(1000L, 9)]
        [DataRow(1024L, 10)]
        [DataRow(1080L, 10)]
        [DataRow(12000L, 13)]
        [DataTestMethod]
        public void Log2(long value, int expected)
        {
            Assert.AreEqual((byte)expected,MathEx.Log2((ulong)value));
        }
    }
}
