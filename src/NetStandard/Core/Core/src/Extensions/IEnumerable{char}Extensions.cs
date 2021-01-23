using System.Collections.Generic;
using System.Text;

namespace Teronis.Extensions
{
    public static class IEnumerableCharacterExtensions
    {
        public static string ConcatFaster(this IEnumerable<char> source)
        {
            var sb = new StringBuilder();

            foreach (var c in source) {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
