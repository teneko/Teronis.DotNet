using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Extensions.NetStandard
{
    public static class StringExtensions
    {
        public static void ToConsole(this string str) => Console.WriteLine(str);

        public static bool Contains(this string source, string search, StringComparison comparison) => source.IndexOf(search, comparison) >= 0;

        public static string Format(this string text, params object[] args)
        {
            if (text == null)
                return null;

            return string.Format(text, args);
        }

        public static byte[] GetUnicodeBytes(this string input) => Encoding.Unicode.GetBytes(input);

        public static string GetUnicodeString(this byte[] input) => Encoding.Unicode.GetString(input);

        public delegate char ManipulateFirstLetterDelegate(char firstChar);

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string UppercaseFirstLetter(this string source) => ManipulateFirstLetter(source, c => char.ToUpper(c));

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string LowercaseFirstLetter(this string source) => ManipulateFirstLetter(source, c => char.ToLower(c));

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string ManipulateFirstLetter(this string source, ManipulateFirstLetterDelegate manipulateFirstLetter)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            char[] a = source.ToCharArray();
            a[0] = manipulateFirstLetter(a[0]);
            return new string(a);
        }

        public static string TrimEnd(this string source, string value)
            => (source?.EndsWith(value) ?? false) ? source.Remove(source.LastIndexOf(value, StringComparison.Ordinal)) : source;

        public static string TrimStart(this string source, string value)
            => (source?.StartsWith(value) ?? false) ? source.Remove(source.IndexOf(value, StringComparison.Ordinal), value.Length) : source;

        public static string DecodeHtml(this string htmlText) => htmlText.Replace("&nbsp;", " ").Replace("\n", "").Replace("\t", "").Trim();
    }
}
