// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            var separator = new StringSeparator("&");

            foreach (string? key in collection.Keys) {
                var keyToEscape = key ?? string.Empty;
                var valueToEscape = collection[key] ?? string.Empty;
                builder.AppendFormat("{0}={1}", Uri.EscapeDataString(keyToEscape), Uri.EscapeDataString(valueToEscape));
                separator.SetSeparator(builder);
            }

            return builder.ToString();
        }
    }
}
