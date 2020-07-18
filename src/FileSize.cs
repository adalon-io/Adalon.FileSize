using System;

namespace Adalon.IO
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct FileSize : IFormattable, IEquatable<FileSize>, IComparable<FileSize>, IComparable
    {
        private readonly long _size;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public FileSize(long size)
        {
            _size = size;
        }

        /// <summary>
        /// 
        /// </summary>
        public long InBytes => _size;

        /// <inheritdoc />
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

        /// <summary>Formats the value of the current instance using the specified format.</summary>
        /// <param name="format">The format to use.   -or-   A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"></see> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.   -or-   A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>The value of the current instance in the specified format.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return FileSizeFormatting.Format(_size, format, FileSizeFormatInfo.GetInstance(formatProvider));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static FileSize operator +(FileSize left, FileSize right)
        {
            return new FileSize(left._size + right._size);
        }

        public static FileSize operator -(FileSize left, FileSize right)
        {
            return new FileSize(left._size - right._size);
        }

        public bool Equals(FileSize other)
        {
            return _size == other._size;
        }

        public override bool Equals(object obj)
        {
            return obj is FileSize other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _size.GetHashCode();
        }

        public static bool operator ==(FileSize left, FileSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(FileSize left, FileSize right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(FileSize other)
        {
            return _size.CompareTo(other._size);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is FileSize other
                ? CompareTo(other)
                : throw new ArgumentException($"Object must be of type {nameof(FileSize)}");
        }

        public static bool operator <(FileSize left, FileSize right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(FileSize left, FileSize right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(FileSize left, FileSize right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(FileSize left, FileSize right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}