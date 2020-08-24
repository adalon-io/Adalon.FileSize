using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Adalon.IO
{
    internal static class Utils
    {
        //256 - AggressiveInlining
        [MethodImpl((MethodImplOptions) 256)]
        public static bool IsNullOrWhiteSpace(string str)
        {
#if NET35
            if (str == null)
                return true;
            
            foreach (var t in str)
            {
                if (!char.IsWhiteSpace(t))
                    return false;
            }

            return true;
#else

            return string.IsNullOrWhiteSpace(str);
#endif
        }
    }
}