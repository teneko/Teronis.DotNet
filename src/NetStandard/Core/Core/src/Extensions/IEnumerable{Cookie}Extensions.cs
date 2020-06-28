using System.Collections.Generic;
using System.Net;

namespace Teronis.Extensions
{
    public static class IEnumerableCookieExtensions
    {

        public static void LetExpire(this IEnumerable<Cookie?> cookies)
        {
            foreach (var cookie in cookies) {
                if (cookie is null) {
                    continue;
                }

                cookie.Expired = true;
            }
        }
    }
}
