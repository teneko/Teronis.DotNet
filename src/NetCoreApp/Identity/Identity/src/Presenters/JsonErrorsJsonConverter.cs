using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Teronis.Identity.Presenters
{
    public class JsonErrorsTextJsonConverter : JsonConverter<JsonErrors>
    {
        public override JsonErrors Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            throw new NotImplementedException();

        public override void Write(Utf8JsonWriter writer, JsonErrors jsonErrors, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(JsonErrors.ErrorsPropertyName);
            writer.WriteStartObject();

            foreach (var jsonError in jsonErrors) {
                writer.WritePropertyName(jsonError.ErrorCode);
                writer.WriteStringValue(jsonError.Error.Message);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}
