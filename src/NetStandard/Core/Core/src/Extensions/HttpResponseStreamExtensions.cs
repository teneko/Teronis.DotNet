// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Net;

namespace Teronis.Extensions
{
    public static class HttpResponseStreamExtensions
    {
        public static string ReadContentAsString(this HttpWebResponse response)
        {
            using var stream = response.GetResponseStream();
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
