using Force.Crc32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Teronis.NetStandard.Extensions
{
    public static class ArrayGenericExtensions
    {
        public static T[] ExcludeNulls<T>(this T[] array) where T : class => ((IEnumerable<T>)array).ExcludeNulls().ToArray();
    }
}
