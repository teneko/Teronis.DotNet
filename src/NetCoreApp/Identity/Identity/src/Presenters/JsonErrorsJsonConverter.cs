using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;

namespace Teronis.Identity.Presenters
{
    public class JsonErrorsJsonConverter : JsonConverter<JsonErrors>
    {
        public override JsonErrors Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, JsonErrors value, JsonSerializerOptions options)
        {
            string rawJson = JsonConvert.SerializeObject(value);

            using (JsonDocument document = JsonDocument.Parse(rawJson)) {
                document.RootElement.WriteTo(writer);
            }
        }
    }
}
