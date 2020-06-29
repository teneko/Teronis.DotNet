using System;
using System.Collections.Specialized;
using System.Text;
using Teronis.Text;

namespace Teronis.Extensions
{
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Concentrates name value collection to one string to become 'key=value[&key2=value2&..]'.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">An key in collection is null.</exception>
        public static string AsPostData(this NameValueCollection collection)
        {
            collection = collection ?? throw new ArgumentNullException(nameof(collection));
            var builder = new StringBuilder();
            var separator = new StringSeparationHelper("&");

            foreach (string? key in collection.Keys) {
                if (key is null) {
                    throw new ArgumentNullException(nameof(key));
                }

                builder.AppendFormat("{0}={1}", Uri.EscapeDataString(key), Uri.EscapeDataString(collection[key]));
                separator.SetStringSeparator(builder);
            }

            return builder.ToString();
        }
    }
}
