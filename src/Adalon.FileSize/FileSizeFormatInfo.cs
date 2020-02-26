using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Adalon.IO
{
    public sealed class FileSizeFormatInfo:ICloneable,IFormatProvider
    {
        private readonly FileSizeCultureData _cultureData;
        private readonly NumberFormatInfo _formatInfo;
        private static  readonly AsyncLocal<FileSizeFormatInfo> CurrentInfoCache = new AsyncLocal<FileSizeFormatInfo>();

        public static FileSizeFormatInfo CurrentInfo
        {
            get
            {
                var ci = Thread.CurrentThread.CurrentCulture;
                var nfi = ci.NumberFormat;
                var fscd = FileSizeCultureData.GetData(null);
                while (!ci.Equals(CultureInfo.InvariantCulture))
                {
                    if (FileSizeCultureData.ContainsData(ci.Name))
                    {
                        fscd = FileSizeCultureData.GetData(ci.Name);
                        break;
                    }

                    ci = ci.Parent;
                }
                if(CurrentInfoCache.Value != null)
                {
                    if (CurrentInfoCache.Value._formatInfo.Equals(nfi)) return CurrentInfoCache.Value;
                }
                var value = new FileSizeFormatInfo(fscd,nfi);
                CurrentInfoCache.Value = value;
                return value;

            }
        }

        public FileSizeFormatInfo(FileSizeCultureData cultureData, NumberFormatInfo formatInfo)
        {
            _cultureData = cultureData;
            _formatInfo = formatInfo;
        }

        public static FileSizeFormatInfo GetInstance(IFormatProvider provider)
        {
            if (provider is FileSizeFormatInfo fileSizeFormatInfo)
            {
                return fileSizeFormatInfo;
            }
            if (provider is CultureInfo cultureInfo)
            {

            }

            if (provider is NumberFormatInfo numberFrFormatInfo)
            {

            }

            if (provider != null &&
                provider.GetFormat(typeof(FileSizeFormatInfo)) is FileSizeFormatInfo requestedFormatInfo)
            {
                return requestedFormatInfo;
            }

            return CurrentInfo;

        }

        public object GetFormat(Type formatType)
        {
            return formatType == typeof(FileSizeFormatInfo) ? this : null;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public bool Equals(FileSizeFormatInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_cultureData, other._cultureData) && Equals(_formatInfo, other._formatInfo);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is FileSizeFormatInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_cultureData != null ? _cultureData.GetHashCode() : 0) * 397) ^ (_formatInfo != null ? _formatInfo.GetHashCode() : 0);
            }
        }
    }
}
