using System.IO;
using System.Net;

namespace Teronis.Extensions
{
    public static class HttpResponseStreamExtensions
    {
        public static string ReadContentAsString(this HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
