using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Tools.NetStandard
{
    public static class DateTimeTools
    {
        private static DateTime ECMAScriptTimestampAsInitialDateTime = new DateTime(1970, 1, 1);

        /// <summary>Returns a .Net <see cref="DateTime"/> representation of a time value from the ECMAScript specification.</summary>
        public static DateTime ECMAScriptTimestampToDateTime(double timeValue)
            => ECMAScriptTimestampAsInitialDateTime.AddMilliseconds(timeValue);

        /// <summary>Returns a time value from the ECMAScript specification.</summary>
        public static long DateTimeToECMAScriptTimestamp(DateTime dateTime)
            => Convert.ToInt64((dateTime - ECMAScriptTimestampAsInitialDateTime).TotalMilliseconds);
    }
}
