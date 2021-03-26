// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Teronis.Extensions
{
    public static class CookieCollectionExtensions
    {
        public static CookieContainer CopyTo(this CookieCollection cookieCollection, CookieContainer cookieContainer)
        {
            // they will be cloned/copied by implementation
            cookieContainer.Add(cookieCollection);
            return cookieContainer;
        }

        public static CookieContainer CopyIt(this CookieCollection colCookies) => CopyTo(colCookies, new CookieContainer());

        public static CookieCollection Clone(this CookieCollection cookies)
        {
            var colCookies = new CookieCollection();

            foreach (Cookie cookie in (IEnumerable<Cookie>)cookies) {
                colCookies.Add(cookie);
            }

            return colCookies;
        }

        public static CookieCollection Clone(this CookieCollection cookies, SemaphoreSlim sephoCookies)
        {
            var colCookies = new CookieCollection();
            sephoCookies.Wait();

            try {
                foreach (Cookie cookie in (IEnumerable<Cookie>)cookies) {
                    colCookies.Add(cookie);
                }
            } finally {
                sephoCookies.Release();
            }

            return colCookies;
        }

        public static async Task<CookieCollection> CloneAsync(this CookieCollection cookies, SemaphoreSlim sephoCookies)
        {
            var colCookies = new CookieCollection();

            await sephoCookies.WaitAsync();

            try {
                foreach (Cookie cookie in (IEnumerable<Cookie>)cookies) {
                    colCookies.Add(cookie);
                }
            } finally {
                sephoCookies.Release();
            }

            return colCookies;
        }

        public static void LetExpire(this CookieCollection cookies)
        {
            IEnumerable<Cookie?> getCookies()
            {
                foreach (Cookie? cookie in cookies) {
                    yield return cookie;
                }
            }

            getCookies().LetExpire();
        }
    }
}
