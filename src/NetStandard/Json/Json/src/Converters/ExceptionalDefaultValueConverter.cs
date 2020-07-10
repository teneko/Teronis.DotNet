using System;
using Newtonsoft.Json;
using Teronis.Extensions;
using Teronis.Json.Extensions;

namespace Teronis.Json.Converters
{
    public class ExceptionalDefaultValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try {
                var serializerSettings = serializer.GetSettings();
                return reader.Value.ToString().DeserializeJson(objectType, serializerSettings);
            } catch {
                return objectType.GetDefault();
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
