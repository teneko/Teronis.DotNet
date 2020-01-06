using Newtonsoft.Json;

namespace Teronis.Json.Extensions
{
    public static class ObjectExtensions
    {
        public static string SerializeObject(this object obj) => JsonConvert.SerializeObject(obj);
        public static string SerializeObject(this object obj, JsonSerializerSettings settings) => JsonConvert.SerializeObject(obj, settings);
    }
}
