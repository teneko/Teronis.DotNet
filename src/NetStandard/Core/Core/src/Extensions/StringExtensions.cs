// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Text;

namespace Teronis.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string search, StringComparison comparison)
            => source.IndexOf(search, comparison) >= 0;

        public static string? Format(this string text, params object[] args)
        {
            if (text == null) {
                return null;
            }

            return string.Format(text, args);
        }

        public static byte[] GetUnicodeBytes(this string input)
            => Encoding.Unicode.GetBytes(input);

        public static string GetUnicodeString(this byte[] input)
            => Encoding.Unicode.GetString(input);

        public delegate char ManipulateFirstCharacterDelegate(char firstChar);

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string UpperFirst(this string source)
            => ManipulateFirst(source, c => char.ToUpper(c));

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string LowerFirst(this string source)
            => ManipulateFirst(source, c => char.ToLower(c));

        /// <summary>
        /// Returns the input string with the first character converted to uppercase, or mutates any nulls passed into string.Empty
        /// </summary>
        public static string ManipulateFirst(this string source, ManipulateFirstCharacterDelegate manipulateFirstCharacter)
        {
            if (string.IsNullOrEmpty(source)) {
                return source;
            }

            char[] a = source.ToCharArray();
            a[0] = manipulateFirstCharacter(a[0]);
            return new string(a);
        }

        [return: NotNullIfNotNull("source")]
        public static string? TrimEnd(this string source, string value, StringComparison comparisonType)
        {
            if (source?.EndsWith(value, comparisonType) ?? false) {
                return source.Remove(source.LastIndexOf(value, comparisonType));
            } else {
                return source;
            }
        }

        [return: NotNullIfNotNull("source")]
        public static string? TrimEnd(this string source, string value) =>
            TrimEnd(source, value, StringComparison.Ordinal);

        [return: NotNullIfNotNull("source")]
        public static string? TrimStart(this string source, string value, StringComparison comparisonType)
        {
            if (source?.StartsWith(value, comparisonType) ?? false) {
                return source.Remove(source.IndexOf(value, comparisonType), value.Length);
            } else {
                return source;
            }
        }

        [return: NotNullIfNotNull("source")]
        public static string? TrimStart(this string source, string value) =>
            TrimStart(source, value, StringComparison.Ordinal);

        public static string DecodeHtml(this string htmlText)
            => htmlText.Replace("&nbsp;", " ").Replace("\n", "").Replace("\t", "").Trim();

        public static SecureString ToSecureString(this string password)
        {
            var securedString = new SecureString();

            foreach (var passwordChar in password) {
                securedString.AppendChar(passwordChar);
            }

            return securedString;
        }

        /// <summary>
        /// Converts umlauts to AE/Ae/ae, OE/Oe/oe and UE/Ue/ue.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ConvertUmlauts(this string content)
        {
            var builder = new StringBuilder();
            var wasLastLetterCapitalUmlaut = false;

            foreach (var letter in content) {
                if (letter == 'Ä') {
                    builder.Append('A');
                    builder.Append('E');
                } else if (letter == 'ä') {
                    builder.Append('a');
                    builder.Append('e');
                } else if (letter == 'Ö') {
                    builder.Append('O');
                    builder.Append('E');
                } else if (letter == 'ö') {
                    builder.Append('o');
                    builder.Append('e');
                } else if (letter == 'Ü') {
                    builder.Append('U');
                    builder.Append('E');
                } else if (letter == 'ü') {
                    builder.Append('u');
                    builder.Append('e');
                } else {
                    if (char.IsLower(letter) && wasLastLetterCapitalUmlaut) {
                        var lastIndex = builder.Length - 1;
                        var lastCapitalLetter = builder[lastIndex];
                        builder.Remove(lastIndex, 1);
                        builder.Append(char.ToLower(lastCapitalLetter));
                    }

                    builder.Append(letter);
                    wasLastLetterCapitalUmlaut = false;
                    continue;
                }

                if (char.IsUpper(letter)) {
                    wasLastLetterCapitalUmlaut = true;
                } else {
                    wasLastLetterCapitalUmlaut = false;
                }
            }

            return builder.ToString();
        }
    }
}
