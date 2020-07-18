using System;
using System.Collections.Generic;
using System.Globalization;
#if NET40
using System.Runtime.Remoting.Messaging;
#endif
using System.Text;
using System.Threading;

namespace Adalon.IO
{
    public sealed class FileSizeFormatInfo:ICloneable,IFormatProvider, IEquatable<FileSizeFormatInfo>
    {
        private readonly FileSizeCultureData _cultureData;
        private readonly NumberFormatInfo _numberFormat;

#if NET40
        private const string CurrentInfoCacheStoreKey = "__Adalon.IO.FileSizeFormatInfo.CurrentInfoCacheStoreKey";
#else
        private static  readonly AsyncLocal<FileSizeFormatInfo> CurrentInfoCacheStore = new AsyncLocal<FileSizeFormatInfo>();
#endif

        

        private static FileSizeFormatInfo CurrentInfoCache
        {
#if NET40
            get => CallContext.LogicalGetData(CurrentInfoCacheStoreKey) as FileSizeFormatInfo;
            set => CallContext.LogicalSetData(CurrentInfoCacheStoreKey, value);
#else
            get => CurrentInfoCacheStore.Value;
            set => CurrentInfoCacheStore.Value = value;
#endif
        }

        public static FileSizeFormatInfo CurrentInfo
        {
            get
            {
                var currentThreadCulture = Thread.CurrentThread.CurrentCulture;
                var currentNumberFormat = currentThreadCulture.NumberFormat;
                var cultureData = FileSizeCultureData.GetData(null);
                while (!currentThreadCulture.Equals(CultureInfo.InvariantCulture))
                {
                    if (FileSizeCultureData.ContainsData(currentThreadCulture.Name))
                    {
                        cultureData = FileSizeCultureData.GetData(currentThreadCulture.Name);
                        break;
                    }

                    currentThreadCulture = currentThreadCulture.Parent;
                }
                if(CurrentInfoCache != null)
                {
                    if (CurrentInfoCache._numberFormat.Equals(currentNumberFormat)) return CurrentInfoCache;
                }
                var value = new FileSizeFormatInfo(cultureData,currentNumberFormat);
                CurrentInfoCache = value;
                return value;

            }
        }

        internal NumberFormatInfo NumberFormat => _numberFormat;

        internal FileSizeCultureData CultureData => _cultureData;

        public FileSizeFormatInfo(FileSizeCultureData cultureData, NumberFormatInfo numberFormat)
        {
            _cultureData = cultureData;
            _numberFormat = numberFormat;
        }

        public static FileSizeFormatInfo GetInstance(IFormatProvider provider)
        {
            if (provider is FileSizeFormatInfo fileSizeFormatInfo)
            {
                return fileSizeFormatInfo;
            }
            if (provider is CultureInfo cultureInfo)
            {
                var cultureData = FileSizeCultureData.GetData(null);
                while (!cultureInfo.Equals(CultureInfo.InvariantCulture))
                {
                    if (FileSizeCultureData.ContainsData(cultureInfo.Name))
                    {
                        cultureData = FileSizeCultureData.GetData(cultureInfo.Name);
                        break;
                    }
                    cultureInfo = cultureInfo.Parent;
                }
                return new FileSizeFormatInfo(cultureData, cultureInfo.NumberFormat);
            }

            if (provider is NumberFormatInfo numberFrFormatInfo)
            {
                var currentFormat = CurrentInfo;
                return new FileSizeFormatInfo(currentFormat.CultureData,numberFrFormatInfo);
            }

            if (provider != null &&
                provider.GetFormat(typeof(FileSizeFormatInfo)) is FileSizeFormatInfo requestedFormatInfo)
            {
                return requestedFormatInfo;
            }

            return CurrentInfo;

        }

        /// <inheritdoc />
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(FileSizeFormatInfo) ? this : null;
        }

        /// <inheritdoc />
        public object Clone()
        {
            return new FileSizeFormatInfo(_cultureData,_numberFormat);
        }

        /// <inheritdoc />
        public bool Equals(FileSizeFormatInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(_cultureData, other._cultureData) && Equals(_numberFormat, other._numberFormat);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is FileSizeFormatInfo other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                return ((_cultureData != null ? _cultureData.GetHashCode() : 0) * 397) ^ (_numberFormat != null ? _numberFormat.GetHashCode() : 0);
            }
        }
    }
}
