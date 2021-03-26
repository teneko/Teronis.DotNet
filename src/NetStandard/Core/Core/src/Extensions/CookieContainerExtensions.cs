// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Teronis.Utils;

namespace Teronis.Extensions
{
    public static class CookieContainerExtensions
    {
        public static void PrintCookies(this CookieContainer cookieContainer)
        {
            try {
                var domainTableFieldName = "m_domainTable";

                if (!(cookieContainer.GetType().InvokeMember("m_domainTable",
                    BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance,
                    null, cookieContainer, new object[] { }) is Hashtable domainTable)) {
                    throw new ArgumentNullException($"Field {domainTableFieldName} is not existing.");
                }

                foreach (var key in domainTable.Keys) {
                    if (key?.ToString()?.StartsWith(".") ?? true) {
                        continue;
                    }

                    // Look for http cookies.
                    if (cookieContainer.GetCookies(
                        new Uri(string.Format("http://{0}/", key))).Count > 0) {
                        Console.WriteLine(cookieContainer.Count + " HTTP COOKIES FOUND:");
                        Console.WriteLine("----------------------------------");
                        foreach (Cookie? cookie in cookieContainer.GetCookies(new Uri(string.Format("http://{0}/", key)))) {
                            if (cookie is null) {
                                continue;
                            }

                            Console.WriteLine("Name = {0} ; Value = {1} ; Domain = {2}",
                                cookie.Name, cookie.Value, cookie.Domain);
                        }
                    }

                    // Look for https cookies
                    if (cookieContainer.GetCookies(
                        new Uri(string.Format("https://{0}/", key))).Count > 0) {
                        Console.WriteLine(cookieContainer.Count + " HTTPS COOKIES FOUND:");
                        Console.WriteLine("----------------------------------");
                        foreach (Cookie? cookie in cookieContainer.GetCookies(new Uri(string.Format("https://{0}/", key)))) {
                            if (cookie is null) {
                                continue;
                            }

                            Console.WriteLine("Name = {0} ; Value = {1} ; Domain = {2}",
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
            var domainTableFieldName = "m_domainTable";
            var bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var domainTable = ObjectUtils.GetFieldValue<dynamic>(cookieContainer, domainTableFieldName, bindFlags);

            if (domainTable is null) {
                throw new ArgumentNullException($"Field {domainTableFieldName} is not existing.");
            }

            foreach (var entry in domainTable) {
                string key = ObjectUtils.GetPropertyValue<string>(entry, "Key");

                if (key.Contains(domain)) {
                    var value = ObjectUtils.GetPropertyValue<dynamic>(entry, "Value");

                    var internalList = ObjectUtils.GetFieldValue<SortedList<string, CookieCollection>>(value, "_list", bindFlags);
                    foreach (var li in internalList) {
                        if (li is null) {
                            continue;
                        }

                        foreach (Cookie? cookie in li.Value) {
                            if (cookie is null) {
                                continue;
                            }

                            yield return cookie;
                        }
                    }
                }
            }
        }

        public static IEnumerable<Cookie> GetAllCookies(this CookieContainer cookieContainer)
        {
            var domainTableFieldName = "m_domainTable";

            if (!(cookieContainer.GetType().GetField(domainTableFieldName, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(cookieContainer) is Hashtable k)) {
                throw new ArgumentNullException($"Field {domainTableFieldName} is not existing.");
            }

            foreach (DictionaryEntry? element in k) {
                if (element is null) {
                    continue;
                }

                var listFieldName = "m_list";

                if (!(element.Value.GetType().GetField(listFieldName, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(element.Value) is SortedList l)) {
                    throw new ArgumentNullException($"Field {listFieldName} is not existing.");
                }

                foreach (var e in l) {
                    if (e is null) {
                        continue;
                    }

                    var cl = (CookieCollection)((DictionaryEntry)e).Value!;

                    foreach (Cookie? fc in cl) {
                        if (fc is null) {
                            continue;
                        }

                        yield return fc;
                    }
                }
            }
        }
    }
}
