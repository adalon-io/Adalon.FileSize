using System;

namespace Adalon.IO
{
    public readonly struct FileSize:IFormattable
    {
        private readonly long _size;

        public FileSize(long size)
        {
            _size = size;
        }

        public override string ToString()
        {
            return FileSizeFormatting.Format(_size, null, FileSizeFormatInfo.CurrentInfo);
        }

        public string ToString(string format)
        {
            return FileSizeFormatting.Format(_size, format, FileSizeFormatInfo.CurrentInfo);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            return FileSizeFormatting.Format(_size, null, FileSizeFormatInfo.GetInstance(formatProvider));
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return FileSizeFormatting.Format(_size, format, FileSizeFormatInfo.GetInstance(formatProvider));
        }
    }
}
