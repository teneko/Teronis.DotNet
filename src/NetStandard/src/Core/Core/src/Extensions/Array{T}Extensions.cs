using System.Collections.Generic;
using System.Linq;

namespace Teronis.Extensions
{
    public static class ArrayGenericExtensions
    {
        public static T[] ExcludeNulls<T>(this T[] array) where T : class => ((IEnumerable<T>)array).ExcludeNulls().ToArray();
    }
}
