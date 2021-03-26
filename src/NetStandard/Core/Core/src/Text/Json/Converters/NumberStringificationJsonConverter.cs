// Copyright (c) Teroneko.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Teronis.Text.Json.Converters
{
    public class NumberStringificationJsonConverter : JsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(string) == typeToConvert;
        }
        public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number) {
                return reader.TryGetInt64(out long longNumber) ?
                    longNumber.ToString() :
                    reader.GetDouble().ToString();
            }

            if (reader.TokenType == JsonTokenType.String) {
                return reader.GetString();
            }

            using JsonDocument document = JsonDocument.ParseValue(ref reader);
            return document.RootElement.Clone().ToString();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value?.ToString());
    }
}
