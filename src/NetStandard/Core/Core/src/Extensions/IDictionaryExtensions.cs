using System.Collections;

namespace Teronis.Extensions
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
