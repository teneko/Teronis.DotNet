using System.Collections.Generic;
using System.Text;

namespace Teronis.NetStandard.Extensions
{
    public static class IEnumerableCharExtensions
	{
		public static string ConcatFaster(this IEnumerable<char> source)
		{
			var sb = new StringBuilder();

			foreach (var c in source)
				sb.Append(c);

			return sb.ToString();
		}
	}
}
