using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Teronis.NetStandard.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string AsPostData(this NameValueCollection collection)
        {
            var builder = new StringBuilder();
            var separator = new StringSeparationUsage("&");

            foreach (string key in collection.Keys) {
                builder.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(collection[key]));
                separator.SetStringSeparator(builder);
            }

            return builder.ToString();
        }
    }
}
