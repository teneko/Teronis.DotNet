using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Teronis.Extensions.NetStandard
{
    public static class IDictionaryExtenions
    {
        public static bool TryAdd(this IDictionary dictionary, object key, object value)
        {
            if (!dictionary.Contains(key)) {
                dictionary.Add(key, value);
                return true;
            }
            //
            return false;
        }
    }
}
