using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

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
