using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NUnit.Framework;

namespace Adalon.IO.Tests
{
    public class LinuxFormatTests
    {

        private string FormatLinux(FileSize fileSize, string formatString = "L")
        {
            return fileSize.ToString(formatString, CultureInfo.InvariantCulture);
        }

        [TestCase(0L, "0B")]
        [TestCase(1L, "1B")]
        [TestCase(10L, "10B")]
        [TestCase(100L,"100B")]
        [TestCase(999L,"999B")]
        [TestCase(1000L,"1000B")]
        [TestCase(1023L,"1023B")]
        public void Bytes(long value, string expected)
        {
            var fileSize = new FileSize(value);
            Assert.AreEqual(expected, FormatLinux(fileSize), "Exact bytes format does not match expected");
        }

        [TestCase(FileSizeUnit.B,"1B","1B")]
        [TestCase(FileSizeUnit.K,"1.0K", "1.0KiB")]
        [TestCase(FileSizeUnit.M,"1.0M", "1.0MiB")]
        [TestCase(FileSizeUnit.G,"1.0G", "1.0GiB")]
        [TestCase(FileSizeUnit.T,"1.0T", "1.0TiB")]
        [TestCase(FileSizeUnit.P,"1.0P", "1.0PiB")]
        [TestCase(FileSizeUnit.E,"1.0E", "1.0EiB")]
        public void Units(FileSizeUnit unit, string expectedShort, string expectedLong)
        {
            var power = (int) unit;
            var value = 1L << (power * 10);
            var fileSize = new FileSize(value);
            Assert.AreEqual(expectedShort,FormatLinux(fileSize,"l"),"Short unit value format does not match expected");
            Assert.AreEqual(expectedLong,FormatLinux(fileSize,"L"),"Long unit value format does not match expected");
        }

        [TestCase(FileSizeUnit.B,"1B","1B")]
        [TestCase(FileSizeUnit.K,"1.0K", "1.0KB")]
        [TestCase(FileSizeUnit.M,"1.0M", "1.0MB")]
        [TestCase(FileSizeUnit.G,"1.0G", "1.0GB")]
        [TestCase(FileSizeUnit.T,"1.0T", "1.0TB")]
        [TestCase(FileSizeUnit.P,"1.0P", "1.0PB")]
        [TestCase(FileSizeUnit.E,"1.0E", "1.0EB")]
        public void SiUnits(FileSizeUnit unit, string expectedShort, string expectedLong)
        {
            var power = (int) unit;
            var value = (long) Math.Pow(1000,power);
            var fileSize = new FileSize(value);
            Assert.AreEqual(expectedShort,FormatLinux(fileSize,"ld"),"Short unit value format does not match expected");
            Assert.AreEqual(expectedLong,FormatLinux(fileSize,"LD"),"Long unit value format does not match expected");
        }

        [TestCase(0L,"0B", "0B")]
        [TestCase(100L,"100B","-100B")]
        [TestCase(1024L,"1.0KiB","-1.0KiB")]
        [TestCase(10240L,"10KiB","-10KiB")]
        [TestCase(102400L,"100KiB","-100KiB")]
        [TestCase(1024000L,"1000KiB","-1000KiB")]
        [TestCase(1024*1024L,"1.0MiB","-1.0MiB")]
        public void Negation(long value, string expectedPositive, string expectedNegative)
        {
            var positive = new FileSize(value);
            var negative = new FileSize(-1*value);
            Assert.AreEqual(expectedPositive,FormatLinux(positive),"Positive format does not match expected");
            Assert.AreEqual(expectedNegative,FormatLinux(negative),"Negative format does not match expected");
        }

        [TestCase(0L,"0B", "0B")]
        [TestCase(100L,"100B","-100B")]
        [TestCase(1000L,"1.0KB","-1.0KB")]
        [TestCase(10000L,"10KB","-10KB")]
        [TestCase(100000L,"100KB","-100KB")]
        [TestCase(1000000L,"1.0MB","-1.0MB")]
        public void SiNegation(long value, string expectedPositive, string expectedNegative)
        {
            var positive = new FileSize(value);
            var negative = new FileSize(-1*value);
            Assert.AreEqual(expectedPositive,FormatLinux(positive,"LD"),"Positive format does not match expected");
            Assert.AreEqual(expectedNegative,FormatLinux(negative,"LD"),"Negative format does not match expected");
        }

        [TestCase(1024L,"1.0KiB")]
        [TestCase(1025L,"1.1KiB")]
        [TestCase(1126L,"1.1KiB")]
        [TestCase(1127L,"1.2KiB")]
        [TestCase(2047L,"2.0KiB")]
        [TestCase(10239L,"10KiB")]
        [TestCase(1047552L,"1023KiB")]
        [TestCase(1048575L,"1.0MiB")]
        public void Rounding(long value, string expected)
        {
            var fileSize = new FileSize(value);
            Assert.AreEqual(expected,FormatLinux(fileSize),"Rounding result does not match expected");
        }

        [TestCase(1000L,"1.0KB")]
        [TestCase(1001L,"1.1KB")]
        [TestCase(1100L,"1.1KB")]
        [TestCase(1101L,"1.2KB")]
        [TestCase(1999L,"2.0KB")]
        [TestCase(9999L,"10KB")]
        [TestCase(999000L,"999KB")]
        [TestCase(999001L,"1.0MB")]
        public void SiRounding(long value, string expected)
        {
            var fileSize = new FileSize(value);
            Assert.AreEqual(expected,FormatLinux(fileSize,"LD"),"Rounding result does not match expected");
        }
    }
}
