using System;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Tools.NetStandard
{
    public static class DateTimeTools
    {
        private static DateTime ECMAScriptTimeValue = new DateTime(1970, 1, 1);
        //private static DateTime ECMAScriptTimeValueUtc = new DateTime(1970, 1, 1);

        /// <summary>Returns a .Net <see cref="DateTime"/> representation of a time value from the ECMAScript specification.</summary>
        public static DateTime ECMAScriptTimeValueToDateTime(double timeValue)
        {
            return ECMAScriptTimeValue.AddMilliseconds(timeValue);
        }

        /// <summary>Returns a time value from the ECMAScript specification.</summary>
        public static long DateTimeToECMAScriptTimeValue(DateTime dateTime)
        {
            return Convert.ToInt64((dateTime - ECMAScriptTimeValue).TotalMilliseconds);
        }
    }
}
