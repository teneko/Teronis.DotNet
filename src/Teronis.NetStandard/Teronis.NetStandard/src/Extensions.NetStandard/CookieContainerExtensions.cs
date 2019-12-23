using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Teronis.Tools.NetStandard;

namespace Teronis.Extensions.NetStandard
{
    public static class CookieContainerExtensions
    {
        public static void PrintCookies(this CookieContainer cookieContainer)
        {
            try {
                var table = (Hashtable)cookieContainer
                    .GetType().InvokeMember("m_domainTable",
                    BindingFlags.NonPublic |
                    BindingFlags.GetField |
                    BindingFlags.Instance,
                    null,
                    cookieContainer,
                    new object[] { });

                foreach (var key in table.Keys) {
                    if (key.ToString().StartsWith("."))
                        continue;

                    // Look for http cookies.
                    if (cookieContainer.GetCookies(
                        new Uri(string.Format("http://{0}/", key))).Count > 0) {
                        Console.WriteLine(cookieContainer.Count + " HTTP COOKIES FOUND:");
                        Console.WriteLine("----------------------------------");
                        foreach (Cookie cookie in cookieContainer.GetCookies(
                            new Uri(string.Format("http://{0}/", key)))) {
                            Console.WriteLine(
                                "Name = {0} ; Value = {1} ; Domain = {2}",
                                cookie.Name, cookie.Value, cookie.Domain);
                        }
                    }

                    // Look for https cookies
                    if (cookieContainer.GetCookies(
                        new Uri(string.Format("https://{0}/", key))).Count > 0) {
                        Console.WriteLine(cookieContainer.Count + " HTTPS COOKIES FOUND:");
                        Console.WriteLine("----------------------------------");
                        foreach (Cookie cookie in cookieContainer.GetCookies(
                            new Uri(string.Format("https://{0}/", key)))) {
                            Console.WriteLine(
                                "Name = {0} ; Value = {1} ; Domain = {2}",
                                cookie.Name, cookie.Value, cookie.Domain);
                        }
                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Uses Reflection to get ALL of the <see cref="Cookie">Cookies</see> where <see cref="Cookie.Domain"/> 
        /// contains part of the specified string. Will return cookies for any subdomain, as well as dotted-prefix cookies. 
        /// </summary>
        /// <param name="cookieContainer">The <see cref="CookieContainer"/> to extract the <see cref="Cookie">Cookies</see> from.</param>
        /// <param name="domain">The string that contains part of the domain you want to extract cookies for.</param>
        /// <returns></returns>
        public static IEnumerable<Cookie> GetCookies(this CookieContainer cookieContainer, string domain)
        {
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var domainTable = ObjectTools.GetFieldValue<dynamic>(cookieContainer, "m_domainTable", bindFlags);

            foreach (var entry in domainTable) {
                string key = ObjectTools.GetPropertyValue<string>(entry, "Key");

                if (key.Contains(domain)) {
                    var value = ObjectTools.GetPropertyValue<dynamic>(entry, "Value");

                    var internalList = ObjectTools.GetFieldValue<SortedList<string, CookieCollection>>(value, "_list", bindFlags);
                    foreach (var li in internalList) {
                        foreach (Cookie cookie in li.Value) {
                            yield return cookie;
                        }
                    }
                }
            }
        }

        public static IEnumerable<Cookie> GetAllCookies(this CookieContainer cookieContainer)
        {
            var k = (Hashtable)cookieContainer.GetType().GetField("m_domainTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cookieContainer);

            foreach (DictionaryEntry element in k) {
                var l = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);

                foreach (var e in l) {
                    var cl = (CookieCollection)((DictionaryEntry)e).Value;

                    foreach (Cookie fc in cl)
                        yield return fc;
                }
            }
        }
    }
}
