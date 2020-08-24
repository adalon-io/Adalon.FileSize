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

        private static readonly string[] LinuxSizeSuffixes =
        {
            "B", "K", "M", "G", "T", "P", "E"
        };

        public static string Format(long value, string format, FileSizeFormatInfo formatInfo)
        {
            if (Utils.IsNullOrWhiteSpace(format)) format = "W";
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
                    case "l":
                        return FormatLinux(value, formatInfo, false, false);
                    case "L":
                        return FormatLinux(value, formatInfo, false, true);
                }
            }

            if (format.Length == 2)
            {
                switch (format)
                {
                    case "ld":
                    case "lD":
                        return FormatLinux(value, formatInfo, true, false);
                    case "Ld":
                    case "LD":
                        return FormatLinux(value, formatInfo, true, true);
                }
            }

            return FormatWindows(value, formatInfo);
        }

        internal static string FormatWindows(long value, FileSizeFormatInfo formatInfo)
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
                //scale value to range of 1 - 1024*1014 to perform exact division
                var shift = Math.Max(0, (int) unit - 2);
                var left = (int) unit - shift;
                bytes >>= (10 * shift);
                // divide the result further, using floating point
                var relative = bytes * (1.0 / Math.Pow(2, left * 10));
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


        internal static string FormatLinux(long value, FileSizeFormatInfo formatInfo, bool useSi, bool useLongSuffix)
        {
            var range = useSi ? 1000 : 1024;
            int negativeFlag = value < 0 ? -1 : 1;
            ulong bytes = (ulong) (value * negativeFlag);
            var unit = FindLinuxSizeUnit(bytes, useSi);
            var builder = new StringBuilder();
            if (unit == FileSizeUnit.B)
            {
                builder.AppendFormat(formatInfo.NumberFormat, "{0:F0}", value);
                builder.Append("B");
                return builder.ToString();
            }
            var unitText = LinuxSizeSuffixes[(int) unit];
            double relative = 0;
            if (useSi)
            {
                int shift = 0;
                while (bytes > 1_000_000)
                {
                    shift++;
                    bytes /= 1000;
                }
                var left = (int) unit - shift;
                relative = bytes * (1.0 / Math.Pow(10, left * 3));
                //Ceiling 999.x moves to next unit bracket
                if (relative >= 999 && Math.Ceiling(relative) >= 1000)
                {
                    relative = 1;
                    unitText = LinuxSizeSuffixes[(int) unit + 1];
                }
            }
            else
            {
                //scale value to range of 1 - 1024*1014 to perform exact division
                var shift = Math.Max(0, (int) unit - 2);
                var left = (int) unit - shift;
                bytes >>= (10 * shift);
                // divide the result further, using floating point
                relative = bytes * (1.0 / Math.Pow(2, left * 10));
                //Ceiling 1023.x moves to next unit bracket
                if (relative >= 1023 && Math.Ceiling(relative) >= 1024)
                {
                    relative = 1;
                    unitText = LinuxSizeSuffixes[(int) unit + 1];
                }
            }
            double computed = 0;
            string computedFormat = "";
            if (relative >= 10)
            {
                computed = Math.Ceiling(relative);
                computedFormat = "F0";
            }
            else
            {
                computed = Math.Ceiling(relative * 10) / 10;
                computedFormat = "F1";
                if (computed >= 10)
                {
                    computedFormat = "F0";
                }
            }
            computed *= negativeFlag;
            builder.Append(computed.ToString(computedFormat, formatInfo.NumberFormat));
            builder.Append(unitText);
            if (useLongSuffix)
            {
                if (!useSi) builder.Append('i');
                builder.Append('B');
            }

            return builder.ToString();

        }

        internal static string FormatNumber(long value, FileSizeFormatInfo formatInfo)
        {
            return value.ToString("F0", formatInfo.NumberFormat);
        }

        private static FileSizeUnit FindWindowsSizeUnit(ulong value)
        {
            // 1 - 1023 bytes represented by exact value
            if (value < 1024) return FileSizeUnit.B;
            var logSize = MathEx.Log2(value);
            var relative = value / (1UL << (logSize / 10 * 10));
            // if relative is in 1000-1023 range - use next unit
            return (FileSizeUnit) (logSize / 10 + (relative < 1000 ? 0 : 1));
        }

        private static FileSizeUnit FindLinuxSizeUnit(ulong value, bool useSi)
        {
            byte logSize = 0;
            if (useSi)
            {
                while (value >= 1000)
                {
                    logSize += 10;
                    value /= 1000;
                }
            }
            else
            {
                logSize = MathEx.Log2(value);
            }

            return (FileSizeUnit) (logSize / 10);
        }
    }
}