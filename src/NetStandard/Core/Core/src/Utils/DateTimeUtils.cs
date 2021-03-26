// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Teronis.Utils
{
    public static class DateTimeUtils
    {
        private static readonly DateTime ECMAScriptTimestampAsInitialDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Returns a .Net <see cref="DateTime"/> representation of a time value from the ECMAScript specification.
        /// </summary>
        public static DateTime ECMAScriptTimestampToDateTime(double timeValue)
            => ECMAScriptTimestampAsInitialDateTime.AddMilliseconds(timeValue);

        /// <summary>
        /// Returns a time value from the ECMAScript specification.
        /// </summary>
        public static long DateTimeToECMAScriptTimestamp(DateTime dateTime)
            => Convert.ToInt64((dateTime - ECMAScriptTimestampAsInitialDateTime).TotalMilliseconds);
    }
}
