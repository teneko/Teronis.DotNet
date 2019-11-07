using System;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.Extensions.NetStandard
{
    public static class NameValueCollectionExtensions
    {
        public static string AsPostData(this NameValueCollection collection)
        {
            var builder = new StringBuilder();
            var separator = new StringSeparationHelper("&");

            foreach (string key in collection.Keys) {
                builder.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(collection[key]));
                separator.SetStringSeparator(builder);
            }

            return builder.ToString();
        }
    }
}
