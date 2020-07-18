using System;
using System.Collections.Generic;
using System.Text;

namespace Adalon.IO
{
    public class FileSizeCultureData
    {
        private static readonly FileSizeCultureData InvariantInstance = new FileSizeCultureData();

        public static FileSizeCultureData GetData(string cultureName)
        {
            if (cultureName == null)
            {
                return InvariantInstance;
            }

            return null;
        }

        public static bool ContainsData(string cultureName)
        {
            return cultureName == null;
        }
    }
}
