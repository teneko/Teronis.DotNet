// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace Teronis.Extensions
{
    public static class UriExtensions
    {
        public static NameValueCollection ParseQueryString(this Uri uri) => HttpUtility.ParseQueryString(uri.Query);

        /// <param name="uri"></param>
        /// <param name="items"></param>
        /// <returns>The return value is a new instance.</returns>
        public static Uri AddQueryParam(this Uri uri, IEnumerable<(string name, string value)> items)
        {
            var httpValueCollection = ParseQueryString(uri);

            foreach (var (name, value) in items) {
                httpValueCollection.Add(name, value);
            }

            var uriBuilder = new UriBuilder(uri);

            // this code block is taken from httpValueCollection.ToString() method
            // and modified so it encodes strings with HttpUtility.UrlEncode
            if (httpValueCollection.Count == 0) {
                uriBuilder.Query = string.Empty;
            } else {
                var stringBuilder = new StringBuilder();

                for (int i = 0; i < httpValueCollection.Count; i++) {
                    string? text = httpValueCollection.GetKey(i);
                    text = HttpUtility.UrlEncode(text);
                    string val = (text != null) ? (text + "=") : string.Empty;
                    string[]? vals = httpValueCollection.GetValues(i);

                    if (stringBuilder.Length > 0) {
                        stringBuilder.Append('&');
                    }

                    if (vals == null || vals.Length == 0) {
                        stringBuilder.Append(val);
                    } else {
                        if (vals.Length == 1) {
                            stringBuilder.Append(val);
                            stringBuilder.Append(HttpUtility.UrlEncode(vals[0]));
                        } else {
                            for (int j = 0; j < vals.Length; j++) {
                                if (j > 0) {
                                    stringBuilder.Append('&');
                                }

                                stringBuilder.Append(val);
                                stringBuilder.Append(HttpUtility.UrlEncode(vals[j]));
                            }
                        }
                    }
                }

                uriBuilder.Query = stringBuilder.ToString();
            }

            return uriBuilder.Uri;
        }
    }
}
