using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Adalon.IO
{
    internal static class FileSizeFormatting
    {
        //TODO: move to culture data
        private static readonly string[] WindowsSizeSuffixes =
        {
            "bytes", "KB", "MB", "GB", "TB", "PB", "EB"
        };

        public static string Format(long value, string format, FileSizeFormatInfo formatInfo)
        {
            if (string.IsNullOrWhiteSpace(format)) format = "W";
            if (formatInfo == null) formatInfo = FileSizeFormatInfo.CurrentInfo;
            if (format.Length == 1)
            {
                switch (format)
                {
                    case "w":
                    case "W":
                        return FormatWindows(value, formatInfo);
                    case "f":
                    case "F":
                        return FormatNumber(value, formatInfo);
                }
            }

            return FormatWindows(value, formatInfo);
        }

        private static string FormatWindows(long value, FileSizeFormatInfo formatInfo)
        {
            if (value == 0) return "0 bytes";
            int negativeFlag = value < 0 ? -1 : 1;
            ulong bytes = (ulong) (value * negativeFlag);
            var unit = FindWindowsSizeUnit(bytes);
            var builder = new StringBuilder();

            if (unit == FileSizeUnit.B)
            {
                builder.AppendFormat(formatInfo.NumberFormat, "{0:F0} bytes", value);
            }
            else
            {
                var unitText = WindowsSizeSuffixes[(int) unit];
                //scale value to range of 1 - 1024*1014 to peform exact division
                var shift = Math.Max(0, (int) unit - 2);
                var left = (int)unit - shift;
                bytes >>= (10 * shift);
                // divide the result further, using floating point
                var relative = bytes * (1.0 / Math.Pow(2, left*10));
                double computed = 0;
                string computedFormat = "";
                if (relative > 999)
                {
                    computed = 999.0;
                    computedFormat = "F0";
                }
                else if (relative >= 100)
                {
                    computed = Math.Floor(relative);
                    computedFormat = "F0";
                }
                else if (relative >= 10)
                {
                    computed = Math.Floor(relative * 10) / 10;
                    computedFormat = "F1";
                }
                else
                {
                    computed = Math.Floor(relative * 100) / 100;
                    computedFormat = "F2";
                }

                computed *= negativeFlag;
                builder.Append(computed.ToString(computedFormat, formatInfo.NumberFormat));
                builder.Append(" ");
                builder.Append(unitText);
            }
            
            return builder.ToString();
        }

        private static FileSizeUnit FindWindowsSizeUnit(ulong value)
        {
            // 1 - 1023 bytes represented by exact value
            if (value < 1024) return FileSizeUnit.B;
            var logSize = MathEx.Log2(value);
            var relative = value / (1UL << (logSize/10 *10));
            // if relative is in 1000-1023 range - use next unit
            return (FileSizeUnit) (logSize / 10 + (relative < 1000 ? 0 : 1));
        }

        private static string FormatNumber(long value, FileSizeFormatInfo formatInfo)
        {
            return value.ToString("F0", formatInfo.NumberFormat);
        }
    }
}