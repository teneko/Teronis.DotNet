using System.Collections.Generic;
using System.Net;

namespace Teronis.Extensions.NetStandard
{
    public static class IEnumerableCookieExtensions
    {

        public static void LetExpire(this IEnumerable<Cookie> cookies)
        {
            foreach (var cookie in cookies)
                cookie.Expired = true;
        }
    }
}
