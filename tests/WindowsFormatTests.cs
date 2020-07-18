using System.Globalization;
using NUnit.Framework;

namespace Adalon.IO.Tests
{
    public class WindowsFormatTests
    {
        private string FormatWindows(FileSize fileSize)
        {
            return fileSize.ToString("W", CultureInfo.InvariantCulture);
        }

        [TestCase(0L, "0 bytes")]
        [TestCase(1L, "1 bytes")]
        [TestCase(10L, "10 bytes")]
        [TestCase(100L,"100 bytes")]
        [TestCase(999L,"999 bytes")]
        public void Bytes(long value, string expected)
        {
            var fileSize = new FileSize(value);
            Assert.AreEqual(expected, FormatWindows(fileSize), "Exact bytes format does not match expected");
        }

        [TestCase(1024*999L,1024L,"999 KB","0.97 MB")]
        [TestCase(1024*1024*999L,1024*1024*5L, "999 MB", "0.98 GB")]
        [TestCase(1024*1024*1024*999L,1024*1024*1024*15L, "999 GB", "0.99 TB")]
        public void Upscale(long value,long increment, string expectedFormat, string expectedUpscale)
        {
            var fileSize = new FileSize(value);
            Assert.AreEqual(expectedFormat, FormatWindows(fileSize), "Original value format does not match expected");
            var incremented = new FileSize(value + increment);
            Assert.AreEqual(expectedUpscale,FormatWindows(incremented),"Upscaled value format does not match expected");
        }

        [TestCase(0L,"0 bytes", "0 bytes")]
        [TestCase(100L,"100 bytes","-100 bytes")]
        [TestCase(1024L,"1.00 KB","-1.00 KB")]
        [TestCase(10240L,"10.0 KB","-10.0 KB")]
        [TestCase(102400L,"100 KB","-100 KB")]
        [TestCase(1024000L,"0.97 MB","-0.97 MB")]
        public void Negation(long value, string expectedPositive, string expectedNegative)
        {
            var positive = new FileSize(value);
            var negative = new FileSize(-1*value);
            Assert.AreEqual(expectedPositive,FormatWindows(positive),"Positive format does not match expected");
            Assert.AreEqual(expectedNegative,FormatWindows(negative),"Positive format does not match expected");
        }

        [TestCase(FileSizeUnit.B,"1 bytes")]
        [TestCase(FileSizeUnit.K,"1.00 KB")]
        [TestCase(FileSizeUnit.M,"1.00 MB")]
        [TestCase(FileSizeUnit.G,"1.00 GB")]
        [TestCase(FileSizeUnit.T,"1.00 TB")]
        [TestCase(FileSizeUnit.P,"1.00 PB")]
        [TestCase(FileSizeUnit.E,"1.00 EB")]
        public void Units(FileSizeUnit unit, string expected)
        {
            var power = (int) unit;
            var value = 1L << (power * 10);
            var fileSize = new FileSize(value);
            Assert.AreEqual(expected,FormatWindows(fileSize),"Unit value format does not match expected");
        }

        [TestCase(1034L,"1.00 KB")]
        [TestCase(1035L,"1.01 KB")]
        [TestCase(10342L,"10.0 KB")]
        [TestCase(10343L,"10.1 KB")]
        [TestCase(103423L,"100 KB")]
        [TestCase(103424L,"101 KB")]
        [TestCase(1023999L,"999 KB")]
        [TestCase(1024000L,"0.97 MB")]
        [TestCase(1036288L,"0.98 MB")]
        [TestCase(1047522L,"0.99 MB")]
        public void Rounding(long value, string expected)
        {
            var fileSize = new FileSize(value);
            Assert.AreEqual(expected,FormatWindows(fileSize),"Rounding result does not match expected");
        }
    }
}
